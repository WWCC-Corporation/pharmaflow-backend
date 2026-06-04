using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Features.Reportes.DTOs;
using PharmaFlow.Application.Features.Reportes.Handlers;
using PharmaFlow.Application.Features.Reportes.Queries;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Infrastructure.Context;

namespace PharmaFlow.Infrastructure.Repositories.Reportes;

public class ReporteVentasReader : IReporteVentasReader
{
    private readonly PharmaFlowDbContext dbContext;

    public ReporteVentasReader(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<ResumenVentasDto> ObtenerResumenAsync(ObtenerResumenVentasQuery query, CancellationToken cancellationToken)
    {
        var ventas = dbContext.Ventas
            .AsNoTracking()
            .Where(venta => venta.Estado == EstadoVenta.completada);

        if (query.Desde.HasValue)
        {
            ventas = ventas.Where(venta => venta.Fecha >= query.Desde.Value);
        }

        if (query.Hasta.HasValue)
        {
            ventas = ventas.Where(venta => venta.Fecha <= query.Hasta.Value);
        }

        var totalVentas = await ventas.CountAsync(cancellationToken);
        var totalVendido = await ventas.SumAsync(venta => venta.MontoTotal, cancellationToken);

        return new ResumenVentasDto
        {
            TotalVentas = totalVentas,
            TotalVendido = totalVendido,
            PromedioVenta = totalVentas == 0 ? 0 : totalVendido / totalVentas
        };
    }
}
