using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Reportes.Queries;
using PharmaFlow.Application.Features.Reportes.Handlers;

namespace PharmaFlow.Persistence.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportesController : ControllerBase
{
    private readonly ObtenerReporteVentasHandler _handler;

    public ReportesController(ObtenerReporteVentasHandler handler)
    {
        _handler = handler;
    }

    [HttpGet("ventas-por-fecha")]
    public async Task<IActionResult> Get([FromQuery] DateTime inicio, [FromQuery] DateTime fin)
    {
        var query = new ObtenerReporteVentasQuery { FechaInicio = inicio, FechaFin = fin };
        var resultado = await _handler.Handle(query);
        
        return Ok(resultado);
    }
}
