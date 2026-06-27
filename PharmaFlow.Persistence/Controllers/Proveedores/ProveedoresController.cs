using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Proveedores.Commands;
using PharmaFlow.Application.Proveedores.Handlers;
using PharmaFlow.Application.Proveedores.Queries;

namespace PharmaFlow.Persistence.Controllers.Proveedores;

[ApiController]
[Route("api/proveedores")]
public class ProveedoresController : ControllerBase
{
    private readonly RegistrarProveedorHandler registrarProveedorHandler;
    private readonly ActualizarProveedorHandler actualizarProveedorHandler;
    private readonly DesactivarProveedorHandler desactivarProveedorHandler;
    private readonly ListarProveedoresHandler listarProveedoresHandler;
    private readonly ObtenerProveedorPorIdHandler obtenerProveedorPorIdHandler;

    public ProveedoresController(
        RegistrarProveedorHandler registrarProveedorHandler,
        ActualizarProveedorHandler actualizarProveedorHandler,
        DesactivarProveedorHandler desactivarProveedorHandler,
        ListarProveedoresHandler listarProveedoresHandler,
        ObtenerProveedorPorIdHandler obtenerProveedorPorIdHandler)
    {
        this.registrarProveedorHandler = registrarProveedorHandler;
        this.actualizarProveedorHandler = actualizarProveedorHandler;
        this.desactivarProveedorHandler = desactivarProveedorHandler;
        this.listarProveedoresHandler = listarProveedoresHandler;
        this.obtenerProveedorPorIdHandler = obtenerProveedorPorIdHandler;
    }

    [HttpPost]
    public async Task<IActionResult> Registrar([FromBody] RegistrarProveedorCommand command, CancellationToken cancellationToken)
    {
        try
        {
            var proveedor = await registrarProveedorHandler.Handle(command, cancellationToken);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = proveedor.Id }, proveedor);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] ActualizarProveedorCommand command, CancellationToken cancellationToken)
    {
        command.Id = id;

        try
        {
            var proveedor = await actualizarProveedorHandler.Handle(command, cancellationToken);

            return Ok(proveedor);
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] ListarProveedoresQuery query, CancellationToken cancellationToken)
    {
        var proveedores = await listarProveedoresHandler.Handle(query, cancellationToken);

        return Ok(proveedores);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var proveedor = await obtenerProveedorPorIdHandler.Handle(new ObtenerProveedorPorIdQuery { Id = id }, cancellationToken);

        if (proveedor is null)
        {
            return NotFound();
        }

        return Ok(proveedor);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Desactivar(Guid id, CancellationToken cancellationToken)
    {
        try
        {
            await desactivarProveedorHandler.Handle(new DesactivarProveedorCommand { Id = id }, cancellationToken);

            return NoContent();
        }
        catch (InvalidOperationException excepcion)
        {
            return BadRequest(new { mensaje = excepcion.Message });
        }
    }
}
