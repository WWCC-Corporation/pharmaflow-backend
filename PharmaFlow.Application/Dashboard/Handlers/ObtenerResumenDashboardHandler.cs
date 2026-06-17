using PharmaFlow.Application.Dashboard.DTOs;
using PharmaFlow.Application.Dashboard.Queries;

namespace PharmaFlow.Application.Dashboard.Handlers;

public class ObtenerResumenDashboardHandler
{
    private readonly IDashboardReader dashboardReader;

    public ObtenerResumenDashboardHandler(IDashboardReader dashboardReader)
    {
        this.dashboardReader = dashboardReader;
    }

    public Task<DashboardResumenDto> Handle(ObtenerResumenDashboardQuery query, CancellationToken cancellationToken)
    {
        return dashboardReader.ObtenerResumenAsync(query, cancellationToken);
    }
}
