namespace PharmaFlow.Domain.Ports.Repositories;
using System;
using System.Threading;
using System.Threading.Tasks;
using Entities;

public interface IStockLoteRepository
{
    Task<StockLote?> ObtenerPorSucursalYLoteAsync(Guid idSucursal, Guid idLote, CancellationToken cancellationToken = default);
    
    Task AgregarAsync(StockLote stockLote, CancellationToken cancellationToken = default);
    
    Task ActualizarAsync(StockLote stockLote, CancellationToken cancellationToken = default);
}