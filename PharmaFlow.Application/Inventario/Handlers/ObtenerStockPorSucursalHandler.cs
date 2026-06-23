using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PharmaFlow.Application.Inventario.Queries;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
namespace PharmaFlow.Application.Inventario.Handlers;

public class ObtenerStockPorSucursalHandler
{
    private readonly IInventarioReader _inventarioReader;

    public ObtenerStockPorSucursalHandler(IInventarioReader inventarioReader)
    {
        _inventarioReader = inventarioReader;
    }

    public async Task<IEnumerable<VStockPorProducto>> EjecutarAsync(ObtenerStockPorSucursalQuery query, CancellationToken cancellationToken = default)
    {
        if (query.IdSucursal == System.Guid.Empty)
        {
            throw new System.ArgumentException("El ID de la sucursal es inválido.");
        }
        
        return await _inventarioReader.ObtenerStockPorSucursalAsync(query.IdSucursal, cancellationToken);
    }
}