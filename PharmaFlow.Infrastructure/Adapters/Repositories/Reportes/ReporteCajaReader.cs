using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Handlers;
using PharmaFlow.Application.Reportes.Queries;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Reportes;

public class ReporteCajaReader : IReporteCajaReader
{
    private static readonly TipoMovimientoCaja[] TiposIngreso =
    [
        TipoMovimientoCaja.APERTURA,
        TipoMovimientoCaja.VENTA_EFECTIVO_INGRESO,
        TipoMovimientoCaja.INGRESO_MANUAL,
        TipoMovimientoCaja.ANULACION_EGRESO
    ];

    private static readonly TipoMovimientoCaja[] TiposEgreso =
    [
        TipoMovimientoCaja.VUELTO_SALIDA,
        TipoMovimientoCaja.EGRESO_MANUAL,
        TipoMovimientoCaja.ANULACION_INGRESO
    ];

    private readonly PharmaFlowDbContext dbContext;

    public ReporteCajaReader(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ResumenCajaDto> ObtenerResumenAsync(ObtenerResumenCajaQuery query, CancellationToken cancellationToken)
    {
        var movimientos = dbContext.MovimientosCajas.AsNoTracking();

        if (query.IdSucursal.HasValue)
        {
            movimientos = movimientos.Where(movimiento => movimiento.IdSucursal == query.IdSucursal.Value);
        }

        if (query.Desde.HasValue)
        {
            movimientos = movimientos.Where(movimiento => movimiento.CreatedAt >= query.Desde.Value.Date);
        }

        if (query.Hasta.HasValue)
        {
            var hastaExclusivo = query.Hasta.Value.Date.AddDays(1);
            movimientos = movimientos.Where(movimiento => movimiento.CreatedAt < hastaExclusivo);
        }

        var totalIngresos = await movimientos
            .Where(movimiento => TiposIngreso.Contains(movimiento.Tipo))
            .Select(movimiento => (decimal?)movimiento.Monto)
            .SumAsync(cancellationToken) ?? 0;
        var totalEgresos = await movimientos
            .Where(movimiento => TiposEgreso.Contains(movimiento.Tipo))
            .Select(movimiento => (decimal?)movimiento.Monto)
            .SumAsync(cancellationToken) ?? 0;

        var movimientosRecientes = await movimientos
            .OrderByDescending(movimiento => movimiento.CreatedAt)
            .Take(10)
            .Select(movimiento => new
            {
                movimiento.Id,
                movimiento.IdSucursal,
                movimiento.Tipo,
                movimiento.Monto,
                movimiento.Descripcion,
                movimiento.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return new ResumenCajaDto
        {
            IdSucursal = query.IdSucursal,
            Desde = query.Desde?.Date,
            Hasta = query.Hasta?.Date,
            TotalMovimientos = await movimientos.CountAsync(cancellationToken),
            TotalIngresos = totalIngresos,
            TotalEgresos = totalEgresos,
            Balance = totalIngresos - totalEgresos,
            Movimientos = movimientosRecientes
                .Select(movimiento => new MovimientoCajaResumenDto
                {
                    Id = movimiento.Id,
                    IdSucursal = movimiento.IdSucursal,
                    Tipo = movimiento.Tipo.ToString(),
                    Monto = movimiento.Monto,
                    Descripcion = movimiento.Descripcion,
                    Fecha = movimiento.CreatedAt
                })
                .ToList()
        };
    }
}
