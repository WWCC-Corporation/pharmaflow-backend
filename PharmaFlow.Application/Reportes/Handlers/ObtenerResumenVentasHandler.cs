using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Queries;

namespace PharmaFlow.Application.Reportes.Handlers;

public class ObtenerResumenVentasHandler
{
    private readonly IReporteVentasReader reporteVentasReader;

    public ObtenerResumenVentasHandler(IReporteVentasReader reporteVentasReader)
    {
        this.reporteVentasReader = reporteVentasReader;
    }

    public Task<ResumenVentasDto> Handle(ObtenerResumenVentasQuery query, CancellationToken cancellationToken)
    {
        return reporteVentasReader.ObtenerResumenAsync(query, cancellationToken);
    }
}
