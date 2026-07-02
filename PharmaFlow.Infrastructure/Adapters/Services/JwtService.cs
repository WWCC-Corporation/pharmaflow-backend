using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using PharmaFlow.Domain.Ports.Services;

namespace PharmaFlow.Infrastructure.Adapters.Services;

public class JwtService : IJwtService
{
    public (string Token, DateTime ExpiraEn) GenerateToken(Guid usuarioId, string correo, string rol)
    {
        var issuer = Environment.GetEnvironmentVariable("Jwt__Issuer") ?? "PharmaFlow.API";
        var audience = Environment.GetEnvironmentVariable("Jwt__Audience") ?? "PharmaFlow.Client";
        var key = Environment.GetEnvironmentVariable("Jwt__Key") ?? "pharmaflow-development-key-change-me";
        var expirationHours = int.TryParse(Environment.GetEnvironmentVariable("Jwt__ExpirationHours"), out var hours) ? hours : 8;
        var expires = DateTime.UtcNow.AddHours(expirationHours);

        var header = new Dictionary<string, object>
        {
            ["alg"] = "HS256",
            ["typ"] = "JWT"
        };

        var payload = new Dictionary<string, object>
        {
            ["sub"] = usuarioId.ToString(),
            ["email"] = correo,
            [ClaimTypes.Role] = rol,
            ["iss"] = issuer,
            ["aud"] = audience,
            ["exp"] = new DateTimeOffset(expires).ToUnixTimeSeconds(),
            ["iat"] = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()
        };

        var unsignedToken = $"{Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(header))}.{Base64UrlEncode(JsonSerializer.SerializeToUtf8Bytes(payload))}";
        var signature = HMACSHA256.HashData(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(unsignedToken));

        return ($"{unsignedToken}.{Base64UrlEncode(signature)}", expires);
    }

    public string GenerateRefreshToken()
    {
        return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        return Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    }
}
