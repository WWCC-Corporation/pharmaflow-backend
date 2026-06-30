using PharmaFlow.Application.Auth.Commands;
using PharmaFlow.Application.Auth.DTOs;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Domain.Ports.Services;

namespace PharmaFlow.Application.Auth.Handlers;

public class RefreshTokenHandler
{
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IJwtService jwtService;

    public RefreshTokenHandler(IRefreshTokenRepository refreshTokenRepository, IJwtService jwtService)
    {
        this.refreshTokenRepository = refreshTokenRepository;
        this.jwtService = jwtService;
    }

    public async Task<AuthResponseDto?> Handle(RefreshTokenCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.RefreshToken))
        {
            return null;
        }

        var refreshToken = await refreshTokenRepository.ObtenerActivoAsync(command.RefreshToken, cancellationToken);

        if (refreshToken is null)
        {
            return null;
        }

        var usuario = refreshToken.Usuario;
        var rol = usuario.IdRolNavigation?.Nombre ?? string.Empty;
        var jwt = jwtService.GenerateToken(usuario.Id, usuario.Correo, rol);
        var nuevoRefreshToken = jwtService.GenerateRefreshToken();

        await refreshTokenRepository.RevocarAsync(refreshToken, nuevoRefreshToken, cancellationToken);
        await refreshTokenRepository.AgregarAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuario.Id,
            Token = nuevoRefreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);

        return new AuthResponseDto
        {
            Token = jwt.Token,
            RefreshToken = nuevoRefreshToken,
            ExpiraEn = jwt.ExpiraEn,
            UsuarioId = usuario.Id,
            Correo = usuario.Correo,
            Rol = rol
        };
    }
}
