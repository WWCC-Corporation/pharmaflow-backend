using PharmaFlow.Application.Features.Reportes.DTOs;
using PharmaFlow.Application.Features.Reportes.Queries;

namespace PharmaFlow.Application.Features.Reportes.Handlers;

public interface IReporteVentasReader
{
    Task<ResumenVentasDto> ObtenerResumenAsync(ObtenerResumenVentasQuery query, CancellationToken cancellationToken);
}
