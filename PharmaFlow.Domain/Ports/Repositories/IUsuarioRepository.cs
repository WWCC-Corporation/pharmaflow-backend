using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface IUsuarioRepository
{
    Task<IReadOnlyList<Usuario>> ListarAsync(CancellationToken cancellationToken);

    Task<Usuario?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Usuario?> ObtenerPorCorreoAsync(string correo, CancellationToken cancellationToken);

    Task<bool> ExisteCorreoAsync(string correo, Guid? idExcluir, CancellationToken cancellationToken);

    Task AgregarAsync(Usuario usuario, IReadOnlyList<Guid> sucursales, CancellationToken cancellationToken);

    Task ActualizarAsync(Usuario usuario, IReadOnlyList<Guid>? sucursales, CancellationToken cancellationToken);
}
