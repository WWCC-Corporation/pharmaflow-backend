using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Dashboard.Handlers;
using PharmaFlow.Application.Dashboard.Queries;

namespace PharmaFlow.Persistence.Controllers.Dashboard;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly ObtenerResumenDashboardHandler obtenerResumenDashboardHandler;

    public DashboardController(ObtenerResumenDashboardHandler obtenerResumenDashboardHandler)
    {
        this.obtenerResumenDashboardHandler = obtenerResumenDashboardHandler;
    }

    [HttpGet("resumen")]
    public async Task<IActionResult> ObtenerResumen([FromQuery] ObtenerResumenDashboardQuery query, CancellationToken cancellationToken)
    {
        var resultado = await obtenerResumenDashboardHandler.Handle(query, cancellationToken);

        return Ok(resultado);
    }
}
