using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Queries;

namespace PharmaFlow.Application.Reportes.Handlers;

public interface IReporteInventarioReader
{
    Task<ResumenInventarioDto> ObtenerResumenAsync(ObtenerResumenInventarioQuery query, CancellationToken cancellationToken);
}
