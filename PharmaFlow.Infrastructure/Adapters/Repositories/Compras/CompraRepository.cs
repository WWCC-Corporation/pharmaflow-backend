using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Compras;

public class CompraRepository : ICompraRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public CompraRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task AgregarAsync(Compra compra, CancellationToken cancellationToken)
    {
        await dbContext.Compras.AddAsync(compra, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<Compra>> ListarAsync(Guid? idSucursal, DateTime? desde, DateTime? hasta, CancellationToken cancellationToken)
    {
        var compras = dbContext.Compras
            .AsNoTracking()
            .Include(compra => compra.DetalleCompras)
            .AsQueryable();

        if (idSucursal.HasValue)
        {
            compras = compras.Where(compra => compra.IdSucursal == idSucursal.Value);
        }

        if (desde.HasValue)
        {
            compras = compras.Where(compra => compra.Fecha >= desde.Value);
        }

        if (hasta.HasValue)
        {
            compras = compras.Where(compra => compra.Fecha <= hasta.Value);
        }

        return await compras
            .OrderByDescending(compra => compra.Fecha)
            .ToListAsync(cancellationToken);
    }

    public async Task<Compra?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Compras
            .AsNoTracking()
            .Include(compra => compra.DetalleCompras)
            .FirstOrDefaultAsync(compra => compra.Id == id, cancellationToken);
    }
}
