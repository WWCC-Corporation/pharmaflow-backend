using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Ventas.Handlers;

public interface IVentaRepository
{
    Task<List<Venta>> ListarAsync(CancellationToken cancellationToken);

    Task<Venta?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Venta> CrearAsync(Venta venta, CancellationToken cancellationToken);

    Task<bool> AnularAsync(Guid id, CancellationToken cancellationToken);
}
