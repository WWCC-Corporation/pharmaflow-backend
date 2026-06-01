using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Compras.DTOs.Compras;
using PharmaFlow.Application.Features.Compras.Interfaces;

namespace PharmaFlow.Persistence.Controllers.Compras;

[ApiController]
[Route("api/compras")]
public class ComprasController : ControllerBase
{
    private readonly ICompraService _compraService;

    public ComprasController(ICompraService compraService)
    {
        _compraService = compraService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var compras = await _compraService.GetAllAsync();
        return Ok(compras);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var compra = await _compraService.GetByIdAsync(id);

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
            var compra = await _compraService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = compra.Id },
                compra
            );
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