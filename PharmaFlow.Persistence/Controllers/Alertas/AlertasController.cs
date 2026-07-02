using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Alertas.Commands;
using PharmaFlow.Application.Alertas.Handlers;
using PharmaFlow.Application.Alertas.Queries;

namespace PharmaFlow.Persistence.Controllers.Alertas;

[ApiController]
[Route("api/alertas")]
public class AlertasController : ControllerBase
{
    private readonly ListarAlertasHandler listarAlertasHandler;
    private readonly MarcarAlertaLeidaHandler marcarAlertaLeidaHandler;

    public AlertasController(
        ListarAlertasHandler listarAlertasHandler,
        MarcarAlertaLeidaHandler marcarAlertaLeidaHandler)
    {
        this.listarAlertasHandler = listarAlertasHandler;
        this.marcarAlertaLeidaHandler = marcarAlertaLeidaHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] ListarAlertasQuery query, CancellationToken cancellationToken)
    {
        var alertas = await listarAlertasHandler.Handle(query, cancellationToken);

        return Ok(alertas);
    }

    [HttpPatch("{id:guid}/leer")]
    public async Task<IActionResult> MarcarComoLeida(Guid id, CancellationToken cancellationToken)
    {
        var alerta = await marcarAlertaLeidaHandler.Handle(
            new MarcarAlertaLeidaCommand { Id = id },
            cancellationToken);

        if (alerta is null)
        {
            return NotFound();
        }

        return Ok(alerta);
    }
}
