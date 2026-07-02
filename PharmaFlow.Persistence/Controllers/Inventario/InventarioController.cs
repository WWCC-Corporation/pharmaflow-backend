using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Inventario.Commands;
using PharmaFlow.Application.Inventario.DTOs;
using PharmaFlow.Application.Inventario.Handlers;
using PharmaFlow.Application.Inventario.Queries;

namespace PharmaFlow.Persistence.Controllers.Inventario;

[ApiController]
[Route("api/[controller]")]
public class InventarioController : ControllerBase
{
    private readonly AjustarStockHandler ajustarStockHandler;
    private readonly ObtenerStockPorSucursalHandler obtenerStockPorSucursalHandler;

    public InventarioController(
        AjustarStockHandler ajustarStockHandler,
        ObtenerStockPorSucursalHandler obtenerStockPorSucursalHandler)
    {
        this.ajustarStockHandler = ajustarStockHandler;
        this.obtenerStockPorSucursalHandler = obtenerStockPorSucursalHandler;
    }

    [HttpPost("ajustar")]
    public async Task<IActionResult> AjustarStock([FromBody] AjustarStockRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            var command = new AjustarStockCommand(request);
            var resultado = await ajustarStockHandler.EjecutarAsync(command, cancellationToken);

            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = ex.Message });
        }
    }

    [HttpGet("sucursal/{idSucursal}")]
    public async Task<IActionResult> ObtenerStockPorSucursal(Guid idSucursal, CancellationToken cancellationToken)
    {
        try
        {
            var query = new ObtenerStockPorSucursalQuery(idSucursal);
            var resultado = await obtenerStockPorSucursalHandler.EjecutarAsync(query, cancellationToken);

            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { mensaje = ex.Message });
        }
    }
}
