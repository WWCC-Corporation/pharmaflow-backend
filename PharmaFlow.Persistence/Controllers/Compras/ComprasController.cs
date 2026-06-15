using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Compras.Commands;
using PharmaFlow.Application.Features.Compras.DTOs;
using PharmaFlow.Application.Features.Compras.Handlers;
using PharmaFlow.Application.Features.Compras.Queries;

namespace PharmaFlow.Persistence.Controllers.Compras;

[ApiController]
[Route("api/compras")]
public class ComprasController : ControllerBase
{
    private readonly ListarComprasHandler listarComprasHandler;
    private readonly ObtenerCompraPorIdHandler obtenerCompraPorIdHandler;
    private readonly CrearCompraHandler crearCompraHandler;

    public ComprasController(
        ListarComprasHandler listarComprasHandler,
        ObtenerCompraPorIdHandler obtenerCompraPorIdHandler,
        CrearCompraHandler crearCompraHandler)
    {
        this.listarComprasHandler = listarComprasHandler;
        this.obtenerCompraPorIdHandler = obtenerCompraPorIdHandler;
        this.crearCompraHandler = crearCompraHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var compras = await listarComprasHandler.Handle(new ListarComprasQuery(), cancellationToken);

        return Ok(compras);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var compra = await obtenerCompraPorIdHandler.Handle(
            new ObtenerCompraPorIdQuery { Id = id }, cancellationToken);

        if (compra is null)
        {
            return NotFound(new { Message = "Compra no encontrada." });
        }

        return Ok(compra);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CreateCompraDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var compra = await crearCompraHandler.Handle(
                new CrearCompraCommand { Datos = dto }, cancellationToken);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = compra.Id }, compra);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
