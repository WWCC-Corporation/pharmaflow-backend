using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface IRefreshTokenRepository
{
    Task AgregarAsync(RefreshToken refreshToken, CancellationToken cancellationToken);

    Task<RefreshToken?> ObtenerActivoAsync(string token, CancellationToken cancellationToken);

    Task RevocarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken);

    Task RevocarAsync(RefreshToken refreshToken, string? reemplazadoPor, CancellationToken cancellationToken);
}
