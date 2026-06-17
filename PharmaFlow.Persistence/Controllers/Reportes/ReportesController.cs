using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Reportes.Handlers;
using PharmaFlow.Application.Reportes.Queries;

namespace PharmaFlow.Persistence.Controllers.Reportes;

[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    private readonly ObtenerResumenVentasHandler obtenerResumenVentasHandler;

    public ReportesController(ObtenerResumenVentasHandler obtenerResumenVentasHandler)
    {
        this.obtenerResumenVentasHandler = obtenerResumenVentasHandler;
    }

    [HttpGet("ventas/resumen")]
    public async Task<IActionResult> ObtenerResumenVentas([FromQuery] ObtenerResumenVentasQuery query, CancellationToken cancellationToken)
    {
        var resultado = await obtenerResumenVentasHandler.Handle(query, cancellationToken);

        return Ok(resultado);
    }
}
