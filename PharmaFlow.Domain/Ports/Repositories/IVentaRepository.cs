namespace PharmaFlow.Application.Ventas.Handlers;

using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

public interface IVentaRepository
{
    Task<List<Venta>> ListarAsync(CancellationToken cancellationToken);

    Task<Venta?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Venta> CrearAsync(Venta venta, CancellationToken cancellationToken);

    Task<bool> AnularAsync(Guid id, CancellationToken cancellationToken);
}
