using PharmaFlow.Application.Auth.Commands;
using PharmaFlow.Application.Auth.DTOs;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Domain.Ports.Services;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Auth.Handlers;

public class LoginHandler
{
    private readonly IUsuarioRepository usuarioRepository;
    private readonly IRefreshTokenRepository refreshTokenRepository;
    private readonly IPasswordHasherService passwordHasherService;
    private readonly IJwtService jwtService;

    public LoginHandler(
        IUsuarioRepository usuarioRepository,
        IRefreshTokenRepository refreshTokenRepository,
        IPasswordHasherService passwordHasherService,
        IJwtService jwtService)
    {
        this.usuarioRepository = usuarioRepository;
        this.refreshTokenRepository = refreshTokenRepository;
        this.passwordHasherService = passwordHasherService;
        this.jwtService = jwtService;
    }

    public async Task<AuthResponseDto?> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Request.Correo) || string.IsNullOrWhiteSpace(command.Request.Password))
        {
            return null;
        }

        var usuario = await usuarioRepository.ObtenerPorCorreoAsync(command.Request.Correo.Trim(), cancellationToken);

        if (usuario is null || !usuario.Activo || !passwordHasherService.Verify(command.Request.Password, usuario.PasswordHash))
        {
            return null;
        }

        var rol = usuario.IdRolNavigation?.Nombre ?? string.Empty;
        var jwt = jwtService.GenerateToken(usuario.Id, usuario.Correo, rol);
        var refreshToken = jwtService.GenerateRefreshToken();

        await refreshTokenRepository.AgregarAsync(new RefreshToken
        {
            Id = Guid.NewGuid(),
            UsuarioId = usuario.Id,
            Token = refreshToken,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(7)
        }, cancellationToken);

        return new AuthResponseDto
        {
            Token = jwt.Token,
            RefreshToken = refreshToken,
            ExpiraEn = jwt.ExpiraEn,
            UsuarioId = usuario.Id,
            Correo = usuario.Correo,
            Rol = rol
        };
    }
}
