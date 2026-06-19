using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Handlers;
using PharmaFlow.Application.Reportes.Queries;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Reportes;

public class ReporteVentasReader : IReporteVentasReader
{
    private readonly PharmaFlowDbContext dbContext;

    public ReporteVentasReader(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ResumenVentasDto> ObtenerResumenAsync(ObtenerResumenVentasQuery query, CancellationToken cancellationToken)
    {
        var ventasBase = dbContext.Ventas.AsNoTracking();

        if (query.Desde.HasValue)
        {
            ventasBase = ventasBase.Where(venta => venta.Fecha >= query.Desde.Value.Date);
        }

        if (query.Hasta.HasValue)
        {
            var hastaExclusivo = query.Hasta.Value.Date.AddDays(1);
            ventasBase = ventasBase.Where(venta => venta.Fecha < hastaExclusivo);
        }

        if (query.IdSucursal.HasValue)
        {
            ventasBase = ventasBase.Where(venta => venta.IdSucursal == query.IdSucursal.Value);
        }

        var ventas = ventasBase.Where(venta => venta.Estado == EstadoVenta.completada);
        var totalVentas = await ventas.CountAsync(cancellationToken);
        var totalVendido = await ventas
            .Select(venta => (decimal?)venta.MontoTotal)
            .SumAsync(cancellationToken) ?? 0;
        var ventasAnuladas = await ventasBase
            .CountAsync(venta => venta.Estado == EstadoVenta.anulada, cancellationToken);
        var ventasPorDia = await ventas
            .GroupBy(venta => venta.Fecha.Date)
            .Select(grupo => new VentaPorDiaDto
            {
                Fecha = grupo.Key,
                CantidadVentas = grupo.Count(),
                TotalVendido = grupo.Sum(venta => venta.MontoTotal)
            })
            .OrderBy(venta => venta.Fecha)
            .ToListAsync(cancellationToken);

        return new ResumenVentasDto
        {
            Desde = query.Desde?.Date,
            Hasta = query.Hasta?.Date,
            IdSucursal = query.IdSucursal,
            TotalVentas = totalVentas,
            TotalVendido = totalVendido,
            PromedioVenta = totalVentas == 0 ? 0 : totalVendido / totalVentas,
            VentasAnuladas = ventasAnuladas,
            VentasPorDia = ventasPorDia
        };
    }
}
