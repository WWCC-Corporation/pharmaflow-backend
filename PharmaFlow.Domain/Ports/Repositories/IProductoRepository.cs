using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface IProductoRepository
{
    Task<IReadOnlyList<Producto>> ListarAsync(string? busqueda, bool? soloActivos, CancellationToken cancellationToken);

    Task<Producto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Producto?> ObtenerPorCodigoBarraAsync(string codigoBarra, CancellationToken cancellationToken);

    Task<bool> ExisteCodigoBarraAsync(string codigoBarra, Guid? idExcluir, CancellationToken cancellationToken);

    Task AgregarAsync(Producto producto, CancellationToken cancellationToken);

    Task ActualizarAsync(Producto producto, CancellationToken cancellationToken);
}
