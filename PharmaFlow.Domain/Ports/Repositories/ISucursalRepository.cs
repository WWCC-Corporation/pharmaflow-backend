using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface ISucursalRepository
{
    Task<IReadOnlyList<Sucursale>> ListarAsync(bool? soloActivas, CancellationToken cancellationToken);

    Task<Sucursale?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<bool> ExisteCodigoAsync(string codigo, Guid? idExcluir, CancellationToken cancellationToken);

    Task AgregarAsync(Sucursale sucursal, CancellationToken cancellationToken);

    Task ActualizarAsync(Sucursale sucursal, CancellationToken cancellationToken);
}
