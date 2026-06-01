using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports;
using PharmaFlow.Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaFlow.Infrastructure.Repositories
{
    public class CajaRepository : ICajaRepository
    {
        private readonly PharmaFlowDbContext _context;

        public CajaRepository(PharmaFlowDbContext context)
        {
            _context = context;
        }

        public async Task<TurnosCaja?> GetTurnoAbiertoPorUsuarioAsync(Guid idUsuario)
        {
            return await _context.TurnosCajas
                .FirstOrDefaultAsync(t => t.IdUsuario == idUsuario && t.Abierto == true);
        }

        public async Task<TurnosCaja?> GetTurnoPorIdAsync(Guid idTurnoCaja)
        {
            return await _context.TurnosCajas
                .FirstOrDefaultAsync(t => t.Id == idTurnoCaja);
        }

        public async Task AddTurnoCajaAsync(TurnosCaja turno)
        {
            await _context.TurnosCajas.AddAsync(turno);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTurnoCajaAsync(TurnosCaja turno)
        {
            _context.TurnosCajas.Update(turno);
            await _context.SaveChangesAsync();
        }

        public async Task AddMovimientoCajaAsync(MovimientosCaja movimiento)
        {
            await _context.MovimientosCajas.AddAsync(movimiento);
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> CalcularTotalVentasTurnoAsync(Guid idTurnoCaja)
        {
            // Sumamos las ventas asociadas al turno. Depende de cómo Diego enlazó las Ventas.
            // Asumiendo que Venta tiene IdTurnoCaja o está relacionado.
            // Por ahora, como es un ejemplo base, sumaremos los movimientos tipo "IngresoVenta" o similar si existe.
            // Si no existe relación directa en Venta, devolveremos 0 por ahora para compilar.
            return await Task.FromResult(0m);
        }

        public async Task<decimal> CalcularTotalMovimientosTurnoAsync(Guid idTurnoCaja)
        {
            var ingresos = await _context.MovimientosCajas
                .Where(m => m.IdTurnoCaja == idTurnoCaja && m.Tipo == TipoMovimientoCaja.Ingreso)
                .SumAsync(m => m.Monto);

            var egresos = await _context.MovimientosCajas
                .Where(m => m.IdTurnoCaja == idTurnoCaja && m.Tipo == TipoMovimientoCaja.Egreso)
                .SumAsync(m => m.Monto);

            return ingresos - egresos;
        }
    }
}
