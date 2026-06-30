using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;
namespace PharmaFlow.Infrastructure.Adapters.Repositories.Inventario;

public class StockLoteRepository : IStockLoteRepository
{
    private readonly PharmaFlowDbContext _dbContext;

    public StockLoteRepository(PharmaFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<StockLote?> ObtenerPorSucursalYLoteAsync(Guid idSucursal, Guid idLote, CancellationToken cancellationToken = default)
    {
        // Usamos StockLotes tal cual está en tu DbContext
        return await _dbContext.StockLotes
            .FirstOrDefaultAsync(x => x.IdSucursal == idSucursal && x.IdLote == idLote, cancellationToken);
    }

    public async Task AgregarAsync(StockLote stockLote, CancellationToken cancellationToken = default)
    {
        await _dbContext.StockLotes.AddAsync(stockLote, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(StockLote stockLote, CancellationToken cancellationToken = default)
    {
        _dbContext.StockLotes.Update(stockLote);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
