using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Reportes.Handlers;
using PharmaFlow.Application.Reportes.Queries;

namespace PharmaFlow.Persistence.Controllers.Reportes;

[ApiController]
[Route("api/reportes")]
public class ReportesController : ControllerBase
{
    private readonly ObtenerResumenVentasHandler obtenerResumenVentasHandler;
    private readonly ObtenerResumenInventarioHandler obtenerResumenInventarioHandler;

    public ReportesController(
        ObtenerResumenVentasHandler obtenerResumenVentasHandler,
        ObtenerResumenInventarioHandler obtenerResumenInventarioHandler)
    {
        this.obtenerResumenVentasHandler = obtenerResumenVentasHandler;
        this.obtenerResumenInventarioHandler = obtenerResumenInventarioHandler;
    }

    [HttpGet("ventas/resumen")]
    public async Task<IActionResult> ObtenerResumenVentas([FromQuery] ObtenerResumenVentasQuery query, CancellationToken cancellationToken)
    {
        var resultado = await obtenerResumenVentasHandler.Handle(query, cancellationToken);

        return Ok(resultado);
    }

    [HttpGet("inventario/resumen")]
    public async Task<IActionResult> ObtenerResumenInventario([FromQuery] ObtenerResumenInventarioQuery query, CancellationToken cancellationToken)
    {
        var resultado = await obtenerResumenInventarioHandler.Handle(query, cancellationToken);

        return Ok(resultado);
    }
}
