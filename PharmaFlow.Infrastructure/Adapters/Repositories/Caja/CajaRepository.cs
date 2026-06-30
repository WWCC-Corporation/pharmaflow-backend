using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Caja;

public class CajaRepository : ICajaRepository
{
    private readonly PharmaFlowDbContext _context;

    public CajaRepository(PharmaFlowDbContext context)
    {
        _context = context;
    }

    public async Task<TurnosCaja?> GetTurnoAbiertoPorUsuarioAsync(Guid idUsuario, Guid idSucursal)
    {
        return await _context.TurnosCajas
            .FirstOrDefaultAsync(t => t.IdUsuario == idUsuario && t.IdSucursal == idSucursal && t.Abierto == true);
    }

    public async Task<TurnosCaja?> GetTurnoPorIdAsync(Guid idTurnoCaja)
    {
        return await _context.TurnosCajas.FindAsync(idTurnoCaja);
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

    public async Task<decimal> SumarMovimientosPorTipoAsync(Guid idTurnoCaja, TipoMovimientoCaja tipo)
    {
        return await _context.MovimientosCajas
            .Where(m => m.IdTurnoCaja == idTurnoCaja && m.Tipo == tipo)
            .SumAsync(m => m.Monto);
    }

    public async Task<List<MovimientosCaja>> GetMovimientosPorTurnoAsync(Guid idTurnoCaja)
    {
        return await _context.MovimientosCajas
            .Where(m => m.IdTurnoCaja == idTurnoCaja)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();
    }
}
