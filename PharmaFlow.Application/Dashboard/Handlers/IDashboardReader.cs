using PharmaFlow.Application.Dashboard.DTOs;
using PharmaFlow.Application.Dashboard.Queries;

namespace PharmaFlow.Application.Dashboard.Handlers;

public interface IDashboardReader
{
    Task<DashboardResumenDto> ObtenerResumenAsync(ObtenerResumenDashboardQuery query, CancellationToken cancellationToken);
}
