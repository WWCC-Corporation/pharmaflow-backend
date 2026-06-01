using PharmaFlow.Application.Features.Reportes.Queries;
using PharmaFlow.Application.Features.Reportes.DTOs;
using PharmaFlow.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace PharmaFlow.Application.Features.Reportes.Handlers;

public class ObtenerReporteVentasHandler
{
    private readonly PharmaFlowDbContext _context;

    public ObtenerReporteVentasHandler(PharmaFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<VentaReporteDto>> Handle(ObtenerReporteVentasQuery query)
    {
        var reporte = await _context.Ventas
            .Include(v => v.IdClienteNavigation)
            .Where(v => v.Fecha >= query.FechaInicio && v.Fecha <= query.FechaFin)
            .Select(v => new VentaReporteDto
            {
                Fecha = v.Fecha ?? DateTime.MinValue,
                Cliente = v.IdClienteNavigation != null 
                    ? v.IdClienteNavigation.Nombres + " " + v.IdClienteNavigation.Apellidos 
                    : "Cliente Final",
                MontoTotal = v.MontoTotal
            })
            .ToListAsync();

        return reporte;
    }
}
