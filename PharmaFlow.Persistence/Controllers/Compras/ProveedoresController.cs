using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;
using PharmaFlow.Application.Features.Compras.Interfaces;

namespace PharmaFlow.Persistence.Controllers.Compras;

[ApiController]
[Route("api/proveedores")]
public class ProveedoresController : ControllerBase
{
    private readonly IProveedorService _proveedorService;

    public ProveedoresController(IProveedorService proveedorService)
    {
        _proveedorService = proveedorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var proveedores = await _proveedorService.GetAllAsync();
        return Ok(proveedores);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var proveedor = await _proveedorService.GetByIdAsync(id);

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
            var proveedor = await _proveedorService.CreateAsync(dto);

            return CreatedAtAction(
                nameof(GetById),
                new { id = proveedor.Id },
                proveedor
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProveedorDto dto)
    {
        try
        {
            var proveedor = await _proveedorService.UpdateAsync(id, dto);

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
    public async Task<IActionResult> Delete(Guid id)
    {
        var eliminado = await _proveedorService.DeleteAsync(id);

        if (!eliminado)
        {
            return NotFound(new { Message = "Proveedor no encontrado." });
        }

        return NoContent();
    }
}