using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Queries;

namespace PharmaFlow.Application.Reportes.Handlers;

public class ObtenerResumenInventarioHandler
{
    private readonly IReporteInventarioReader reporteInventarioReader;

    public ObtenerResumenInventarioHandler(IReporteInventarioReader reporteInventarioReader)
    {
        this.reporteInventarioReader = reporteInventarioReader;
    }

    public Task<ResumenInventarioDto> Handle(ObtenerResumenInventarioQuery query, CancellationToken cancellationToken)
    {
        return reporteInventarioReader.ObtenerResumenAsync(query, cancellationToken);
    }
}
