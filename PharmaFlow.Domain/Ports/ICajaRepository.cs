using PharmaFlow.Persistence;
using System;
using System.Threading.Tasks;

namespace PharmaFlow.Domain.Ports
{
    public interface ICajaRepository
    {
        Task<TurnosCaja?> GetTurnoAbiertoPorUsuarioAsync(Guid idUsuario);
        Task<TurnosCaja?> GetTurnoPorIdAsync(Guid idTurnoCaja);
        Task AddTurnoCajaAsync(TurnosCaja turno);
        Task UpdateTurnoCajaAsync(TurnosCaja turno);
        Task AddMovimientoCajaAsync(MovimientosCaja movimiento);
        Task<decimal> CalcularTotalVentasTurnoAsync(Guid idTurnoCaja);
        Task<decimal> CalcularTotalMovimientosTurnoAsync(Guid idTurnoCaja);
    }
}
