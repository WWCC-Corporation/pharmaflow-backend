using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Queries;

namespace PharmaFlow.Application.Reportes.Handlers;

public interface IReporteVentasReader
{
    Task<ResumenVentasDto> ObtenerResumenAsync(ObtenerResumenVentasQuery query, CancellationToken cancellationToken);
}
