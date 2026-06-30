using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Sucursales.Commands;
using PharmaFlow.Application.Sucursales.Handlers;
using PharmaFlow.Application.Sucursales.Queries;

namespace PharmaFlow.Persistence.Controllers.Sucursales;

[ApiController]
[Route("api/sucursales")]
public class SucursalesController : ControllerBase
{
    private readonly ListarSucursalesHandler listarSucursalesHandler;
    private readonly ObtenerSucursalPorIdHandler obtenerSucursalPorIdHandler;
    private readonly CrearSucursalHandler crearSucursalHandler;
    private readonly ActualizarSucursalHandler actualizarSucursalHandler;

    public SucursalesController(
        ListarSucursalesHandler listarSucursalesHandler,
        ObtenerSucursalPorIdHandler obtenerSucursalPorIdHandler,
        CrearSucursalHandler crearSucursalHandler,
        ActualizarSucursalHandler actualizarSucursalHandler)
    {
        this.listarSucursalesHandler = listarSucursalesHandler;
        this.obtenerSucursalPorIdHandler = obtenerSucursalPorIdHandler;
        this.crearSucursalHandler = crearSucursalHandler;
        this.actualizarSucursalHandler = actualizarSucursalHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] ListarSucursalesQuery query, CancellationToken cancellationToken)
    {
        var sucursales = await listarSucursalesHandler.Handle(query, cancellationToken);

        return Ok(sucursales);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var sucursal = await obtenerSucursalPorIdHandler.Handle(new ObtenerSucursalPorIdQuery { Id = id }, cancellationToken);

        if (sucursal is null)
        {
            return NotFound();
        }

        return Ok(sucursal);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CrearSucursalCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var sucursal = await crearSucursalHandler.Handle(command, cancellationToken);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = sucursal.Id }, sucursal);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarSucursalCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;

        try
        {
            var sucursal = await actualizarSucursalHandler.Handle(command, cancellationToken);

            if (sucursal is null)
            {
                return NotFound();
            }

            return Ok(sucursal);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpPatch("{id:guid}/estado")]
    public async Task<IActionResult> CambiarEstado(Guid id, [FromQuery] bool activo, CancellationToken cancellationToken)
    {
        var sucursal = await actualizarSucursalHandler.Handle(new ActualizarSucursalCommand { Id = id, Activo = activo }, cancellationToken);

        if (sucursal is null)
        {
            return NotFound();
        }

        return Ok(sucursal);
    }
}
