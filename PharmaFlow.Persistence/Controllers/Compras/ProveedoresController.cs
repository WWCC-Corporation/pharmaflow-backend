using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Compras.Commands.Proveedores;
using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;
using PharmaFlow.Application.Features.Compras.Queries.Proveedores;

namespace PharmaFlow.Persistence.Controllers.Compras;

[ApiController]
[Route("api/proveedores")]
public class ProveedoresController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProveedoresController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var proveedores = await _mediator.Send(new ListarProveedoresQuery());
        return Ok(proveedores);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var proveedor = await _mediator.Send(new ObtenerProveedorPorIdQuery(id));

        if (proveedor is null)
        {
            return NotFound(new { Message = "Proveedor no encontrado." });
        }

        return Ok(proveedor);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProveedorDto dto)
    {
        try
        {
            var proveedor = await _mediator.Send(new CrearProveedorCommand(dto));

            return CreatedAtAction(
                nameof(GetById),
                new { id = proveedor.Id },
                proveedor
            );
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProveedorDto dto)
    {
        try
        {
            var proveedor = await _mediator.Send(new ActualizarProveedorCommand(id, dto));

            if (proveedor is null)
            {
                return NotFound(new { Message = "Proveedor no encontrado." });
            }

            return Ok(proveedor);
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var eliminado = await _mediator.Send(new DesactivarProveedorCommand(id));

        if (!eliminado)
        {
            return NotFound(new { Message = "Proveedor no encontrado." });
        }

        return NoContent();
    }
}
