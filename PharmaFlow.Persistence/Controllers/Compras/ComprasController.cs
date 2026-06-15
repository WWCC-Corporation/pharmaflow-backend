using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Compras.Commands.Compras;
using PharmaFlow.Application.Features.Compras.DTOs.Compras;
using PharmaFlow.Application.Features.Compras.Queries.Compras;

namespace PharmaFlow.Persistence.Controllers.Compras;

[ApiController]
[Route("api/compras")]
public class ComprasController : ControllerBase
{
    private readonly IMediator _mediator;

    public ComprasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var compras = await _mediator.Send(new ListarComprasQuery());
        return Ok(compras);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var compra = await _mediator.Send(new ObtenerCompraPorIdQuery(id));

        if (compra is null)
        {
            return NotFound(new { Message = "Compra no encontrada." });
        }

        return Ok(compra);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCompraDto dto)
    {
        try
        {
            var compra = await _mediator.Send(new CrearCompraCommand(dto));

            return CreatedAtAction(
                nameof(GetById),
                new { id = compra.Id },
                compra
            );
        }
        catch (ValidationException ex)
        {
            return BadRequest(new { Errors = ex.Errors.Select(e => e.ErrorMessage) });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
