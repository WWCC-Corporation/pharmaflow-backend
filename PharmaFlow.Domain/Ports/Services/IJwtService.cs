namespace PharmaFlow.Domain.Ports.Services;

public interface IJwtService
{
    (string Token, DateTime ExpiraEn) GenerateToken(Guid usuarioId, string correo, string rol);

    string GenerateRefreshToken();
}
