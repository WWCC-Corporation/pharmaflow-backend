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
        throw new NotImplementedException();
    }

    public Task<IEnumerable<VStockFefo>> ObtenerStockFefoAsync(Guid idSucursal, Guid idProducto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}