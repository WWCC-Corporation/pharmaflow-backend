using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Clientes.Commands;
using PharmaFlow.Application.Features.Clientes.DTOs;
using PharmaFlow.Application.Features.Clientes.Handlers;
using PharmaFlow.Application.Features.Clientes.Queries;

namespace PharmaFlow.Persistence.Controllers.Clientes;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly ListarClientesHandler listarClientesHandler;
    private readonly ObtenerClientePorIdHandler obtenerClientePorIdHandler;
    private readonly CrearClienteHandler crearClienteHandler;
    private readonly ActualizarClienteHandler actualizarClienteHandler;
    private readonly DesactivarClienteHandler desactivarClienteHandler;

    public ClientesController(
        ListarClientesHandler listarClientesHandler,
        ObtenerClientePorIdHandler obtenerClientePorIdHandler,
        CrearClienteHandler crearClienteHandler,
        ActualizarClienteHandler actualizarClienteHandler,
        DesactivarClienteHandler desactivarClienteHandler)
    {
        this.listarClientesHandler = listarClientesHandler;
        this.obtenerClientePorIdHandler = obtenerClientePorIdHandler;
        this.crearClienteHandler = crearClienteHandler;
        this.actualizarClienteHandler = actualizarClienteHandler;
        this.desactivarClienteHandler = desactivarClienteHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var clientes = await listarClientesHandler.Handle(new ListarClientesQuery(), cancellationToken);

        return Ok(clientes);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var cliente = await obtenerClientePorIdHandler.Handle(
            new ObtenerClientePorIdQuery { Id = id }, cancellationToken);

        if (cliente is null)
        {
            return NotFound(new { Message = "Cliente no encontrado." });
        }

        return Ok(cliente);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CreateClienteDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cliente = await crearClienteHandler.Handle(
                new CrearClienteCommand { Datos = dto }, cancellationToken);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = cliente.Id }, cliente);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] UpdateClienteDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var cliente = await actualizarClienteHandler.Handle(
                new ActualizarClienteCommand { Id = id, Datos = dto }, cancellationToken);

            if (cliente is null)
            {
                return NotFound(new { Message = "Cliente no encontrado." });
            }

            return Ok(cliente);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Desactivar(Guid id, CancellationToken cancellationToken)
    {
        var eliminado = await desactivarClienteHandler.Handle(
            new DesactivarClienteCommand { Id = id }, cancellationToken);

        if (!eliminado)
        {
            return NotFound(new { Message = "Cliente no encontrado." });
        }

        return NoContent();
    }
}
