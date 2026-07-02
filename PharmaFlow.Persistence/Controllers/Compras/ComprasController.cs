using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Compras.Commands;
using PharmaFlow.Application.Compras.Handlers;
using PharmaFlow.Application.Compras.Queries;

namespace PharmaFlow.Persistence.Controllers.Compras;

[ApiController]
[Route("api/compras")]
public class ComprasController : ControllerBase
{
    private readonly RegistrarCompraHandler registrarCompraHandler;
    private readonly ListarComprasHandler listarComprasHandler;
    private readonly ObtenerCompraPorIdHandler obtenerCompraPorIdHandler;
    private readonly RecepcionarCompraHandler recepcionarCompraHandler;

    public ComprasController(
        RegistrarCompraHandler registrarCompraHandler,
        ListarComprasHandler listarComprasHandler,
        ObtenerCompraPorIdHandler obtenerCompraPorIdHandler,
        RecepcionarCompraHandler recepcionarCompraHandler)
    {
        this.registrarCompraHandler = registrarCompraHandler;
        this.listarComprasHandler = listarComprasHandler;
        this.obtenerCompraPorIdHandler = obtenerCompraPorIdHandler;
        this.recepcionarCompraHandler = recepcionarCompraHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarCompraCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var compra = await registrarCompraHandler.Handle(command, cancellationToken);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = compra.Id }, compra);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] ListarComprasQuery query, CancellationToken cancellationToken)
    {
        var compras = await listarComprasHandler.Handle(query, cancellationToken);

        return Ok(compras);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var compra = await obtenerCompraPorIdHandler.Handle(new ObtenerCompraPorIdQuery { Id = id }, cancellationToken);

        if (compra is null)
        {
            return NotFound();
        }

        return Ok(compra);
    }

    [HttpPost("{id:guid}/recepcionar")]
    public async Task<IActionResult> Recepcionar(Guid id, [FromBody] RecepcionarCompraCommand command, CancellationToken cancellationToken)
    {
        command.IdCompra = id;

        try
        {
            var compra = await recepcionarCompraHandler.Handle(command, cancellationToken);

            return Ok(compra);
        }
        catch (KeyNotFoundException excepcion)
        {
            return NotFound(new { mensaje = excepcion.Message });
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }
}
