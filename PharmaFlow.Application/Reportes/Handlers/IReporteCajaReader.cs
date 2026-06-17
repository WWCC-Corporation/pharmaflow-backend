using PharmaFlow.Application.Reportes.DTOs;
using PharmaFlow.Application.Reportes.Queries;

namespace PharmaFlow.Application.Reportes.Handlers;

public interface IReporteCajaReader
{
    Task<ResumenCajaDto> ObtenerResumenAsync(ObtenerResumenCajaQuery query, CancellationToken cancellationToken);
}
