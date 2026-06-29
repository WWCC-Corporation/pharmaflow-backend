using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Ventas.Commands;
using PharmaFlow.Application.Ventas.DTOs;
using PharmaFlow.Application.Ventas.Handlers;
using PharmaFlow.Application.Ventas.Queries;

namespace PharmaFlow.Persistence.Controllers.Ventas;

[ApiController]
[Route("api/ventas")]
public class VentasController : ControllerBase
{
    private readonly ListarVentasHandler listarVentasHandler;
    private readonly ObtenerVentaPorIdHandler obtenerVentaPorIdHandler;
    private readonly CrearVentaHandler crearVentaHandler;
    private readonly AnularVentaHandler anularVentaHandler;

    public VentasController(
        ListarVentasHandler listarVentasHandler,
        ObtenerVentaPorIdHandler obtenerVentaPorIdHandler,
        CrearVentaHandler crearVentaHandler,
        AnularVentaHandler anularVentaHandler)
    {
        this.listarVentasHandler = listarVentasHandler;
        this.obtenerVentaPorIdHandler = obtenerVentaPorIdHandler;
        this.crearVentaHandler = crearVentaHandler;
        this.anularVentaHandler = anularVentaHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var ventas = await listarVentasHandler.Handle(new ListarVentasQuery(), cancellationToken);

        return Ok(ventas);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var venta = await obtenerVentaPorIdHandler.Handle(
            new ObtenerVentaPorIdQuery { Id = id }, cancellationToken);

        if (venta is null)
        {
            return NotFound(new { Message = "Venta no encontrada." });
        }

        return Ok(venta);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CreateVentaDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var venta = await crearVentaHandler.Handle(
                new CrearVentaCommand { Datos = dto }, cancellationToken);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = venta.Id }, venta);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Anular(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var anulada = await anularVentaHandler.Handle(
                new AnularVentaCommand { Id = id }, cancellationToken);

            if (!anulada)
            {
                return NotFound(new { Message = "Venta no encontrada." });
            }

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }
}
