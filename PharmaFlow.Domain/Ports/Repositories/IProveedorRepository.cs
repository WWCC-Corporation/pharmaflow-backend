using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface IProveedorRepository
{
    Task<Proveedore?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<IReadOnlyList<Proveedore>> ListarAsync(bool? soloActivos, CancellationToken cancellationToken);

    Task<bool> ExisteRucAsync(string ruc, Guid? idExcluir, CancellationToken cancellationToken);

    Task AgregarAsync(Proveedore proveedor, CancellationToken cancellationToken);

    Task ActualizarAsync(Proveedore proveedor, CancellationToken cancellationToken);
}
