using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface IAlertaRepository
{
    Task<IReadOnlyList<Alerta>> ListarAsync(
        Guid? idSucursal,
        bool? soloNoLeidas,
        TipoAlerta? tipo,
        CancellationToken cancellationToken);

    Task<Alerta?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task ActualizarAsync(Alerta alerta, CancellationToken cancellationToken);
}
