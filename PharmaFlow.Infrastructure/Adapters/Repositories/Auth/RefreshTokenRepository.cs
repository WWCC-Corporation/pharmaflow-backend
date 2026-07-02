using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Auth;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public RefreshTokenRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AgregarAsync(RefreshToken refreshToken, CancellationToken cancellationToken)
    {
        await dbContext.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<RefreshToken?> ObtenerActivoAsync(string token, CancellationToken cancellationToken)
    {
        return await dbContext.RefreshTokens
            .Include(refreshToken => refreshToken.Usuario)
            .ThenInclude(usuario => usuario.IdRolNavigation)
            .FirstOrDefaultAsync(refreshToken =>
                refreshToken.Token == token &&
                refreshToken.RevokedAt == null &&
                refreshToken.ExpiresAt > DateTime.UtcNow,
                cancellationToken);
    }

    public async Task RevocarPorUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken)
    {
        var tokens = await dbContext.RefreshTokens
            .Where(refreshToken => refreshToken.UsuarioId == usuarioId && refreshToken.RevokedAt == null)
            .ToListAsync(cancellationToken);

        foreach (var token in tokens)
        {
            token.RevokedAt = DateTime.UtcNow;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task RevocarAsync(RefreshToken refreshToken, string? reemplazadoPor, CancellationToken cancellationToken)
    {
        refreshToken.RevokedAt = DateTime.UtcNow;
        refreshToken.ReplacedByToken = reemplazadoPor;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
