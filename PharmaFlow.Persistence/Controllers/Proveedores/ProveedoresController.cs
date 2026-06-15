using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Proveedores.Commands;
using PharmaFlow.Application.Features.Proveedores.DTOs;
using PharmaFlow.Application.Features.Proveedores.Handlers;
using PharmaFlow.Application.Features.Proveedores.Queries;

namespace PharmaFlow.Persistence.Controllers.Proveedores;

[ApiController]
[Route("api/proveedores")]
public class ProveedoresController : ControllerBase
{
    private readonly ListarProveedoresHandler listarProveedoresHandler;
    private readonly ObtenerProveedorPorIdHandler obtenerProveedorPorIdHandler;
    private readonly CrearProveedorHandler crearProveedorHandler;
    private readonly ActualizarProveedorHandler actualizarProveedorHandler;
    private readonly DesactivarProveedorHandler desactivarProveedorHandler;

    public ProveedoresController(
        ListarProveedoresHandler listarProveedoresHandler,
        ObtenerProveedorPorIdHandler obtenerProveedorPorIdHandler,
        CrearProveedorHandler crearProveedorHandler,
        ActualizarProveedorHandler actualizarProveedorHandler,
        DesactivarProveedorHandler desactivarProveedorHandler)
    {
        this.listarProveedoresHandler = listarProveedoresHandler;
        this.obtenerProveedorPorIdHandler = obtenerProveedorPorIdHandler;
        this.crearProveedorHandler = crearProveedorHandler;
        this.actualizarProveedorHandler = actualizarProveedorHandler;
        this.desactivarProveedorHandler = desactivarProveedorHandler;
    }

    [HttpGet]
    public async Task<IActionResult> Listar(CancellationToken cancellationToken)
    {
        var proveedores = await listarProveedoresHandler.Handle(new ListarProveedoresQuery(), cancellationToken);

        return Ok(proveedores);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObtenerPorId(Guid id, CancellationToken cancellationToken)
    {
        var proveedor = await obtenerProveedorPorIdHandler.Handle(
            new ObtenerProveedorPorIdQuery { Id = id }, cancellationToken);

        if (proveedor is null)
        {
            return NotFound(new { Message = "Proveedor no encontrado." });
        }

        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<IActionResult> Crear([FromBody] CreateProveedorDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var proveedor = await crearProveedorHandler.Handle(
                new CrearProveedorCommand { Datos = dto }, cancellationToken);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = proveedor.Id }, proveedor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Actualizar(Guid id, [FromBody] UpdateProveedorDto dto, CancellationToken cancellationToken)
    {
        try
        {
            var proveedor = await actualizarProveedorHandler.Handle(
                new ActualizarProveedorCommand { Id = id, Datos = dto }, cancellationToken);

            if (proveedor is null)
            {
                return NotFound(new { Message = "Proveedor no encontrado." });
            }

            return Ok(proveedor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Desactivar(Guid id, CancellationToken cancellationToken)
    {
        var eliminado = await desactivarProveedorHandler.Handle(
            new DesactivarProveedorCommand { Id = id }, cancellationToken);

        if (!eliminado)
        {
            return NotFound(new { Message = "Proveedor no encontrado." });
        }

        return NoContent();
    }
}
