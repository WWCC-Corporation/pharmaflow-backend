using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Inventario;

public class InventarioReader : IInventarioReader
{
    private readonly PharmaFlowDbContext _dbContext;

    public InventarioReader(PharmaFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<VStockPorProducto>> ObtenerStockPorSucursalAsync(Guid idSucursal, CancellationToken cancellationToken = default)
    {
        // Usamos VStockPorProductos tal cual está en tu DbContext
        return await _dbContext.VStockPorProductos
            .AsNoTracking()
            .Where(v => v.IdSucursal == idSucursal)
            .ToListAsync(cancellationToken);
    }

    public Task<VStockPorProducto?> ObtenerStockDeProductoEnSucursalAsync(Guid idSucursal, Guid idProducto,
        CancellationToken cancellationToken = default)
    {
        return _dbContext.VStockPorProductos
            .AsNoTracking()
            .FirstOrDefaultAsync(v => v.IdSucursal == idSucursal && v.Id == idProducto, cancellationToken);
    }

    public async Task<IEnumerable<VStockFefo>> ObtenerStockFefoAsync(Guid idSucursal, Guid idProducto, CancellationToken cancellationToken = default)
    {
        return await _dbContext.VStockFefos
            .AsNoTracking()
            .Where(v => v.IdSucursal == idSucursal && v.Id == idProducto)
            .OrderBy(v => v.FechaVencimiento)
            .ToListAsync(cancellationToken);
    }
}
