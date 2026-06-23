using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface ICompraRepository
{
    Task AgregarAsync(Compra compra, CancellationToken cancellationToken);

    Task<IReadOnlyList<Compra>> ListarAsync(Guid? idSucursal, DateTime? desde, DateTime? hasta, CancellationToken cancellationToken);

    Task<Compra?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);
}
