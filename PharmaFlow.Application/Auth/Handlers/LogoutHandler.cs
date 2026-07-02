using PharmaFlow.Application.Auth.Commands;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Auth.Handlers;

public class LogoutHandler
{
    private readonly IRefreshTokenRepository refreshTokenRepository;

    public LogoutHandler(IRefreshTokenRepository refreshTokenRepository)
    {
        this.refreshTokenRepository = refreshTokenRepository;
    }

    public async Task Handle(LogoutCommand command, CancellationToken cancellationToken)
    {
        if (command.UsuarioId == Guid.Empty)
        {
            throw new InvalidOperationException("El usuario es requerido.");
        }

        await refreshTokenRepository.RevocarPorUsuarioAsync(command.UsuarioId, cancellationToken);
    }
}
