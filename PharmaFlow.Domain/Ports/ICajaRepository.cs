using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface ICajaRepository
{
    Task<TurnosCaja?> GetTurnoAbiertoPorUsuarioAsync(Guid idUsuario, Guid idSucursal);
    Task<TurnosCaja?> GetTurnoPorIdAsync(Guid idTurnoCaja);
    Task AddTurnoCajaAsync(TurnosCaja turno);
    Task UpdateTurnoCajaAsync(TurnosCaja turno);
    Task AddMovimientoCajaAsync(MovimientosCaja movimiento);
    Task<decimal> SumarMovimientosPorTipoAsync(Guid idTurnoCaja, TipoMovimientoCaja tipo);
    Task<List<MovimientosCaja>> GetMovimientosPorTurnoAsync(Guid idTurnoCaja);
}
