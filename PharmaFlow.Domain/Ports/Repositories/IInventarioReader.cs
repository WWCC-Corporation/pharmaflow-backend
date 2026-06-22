using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PharmaFlow.Domain.Entities;
namespace PharmaFlow.Domain.Ports.Repositories;

public interface IInventarioReader
{
    
    Task<IEnumerable<VStockPorProducto>> ObtenerStockPorSucursalAsync(Guid idSucursal, CancellationToken cancellationToken = default);
    
    Task<VStockPorProducto?> ObtenerStockDeProductoEnSucursalAsync(Guid idSucursal, Guid idProducto, CancellationToken cancellationToken = default);

    
    Task<IEnumerable<VStockFefo>> ObtenerStockFefoAsync(Guid idSucursal, Guid idProducto, CancellationToken cancellationToken = default);
}