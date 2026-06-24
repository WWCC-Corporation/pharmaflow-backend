using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Inventario.Commands;
using PharmaFlow.Application.Inventario.DTOs;
using PharmaFlow.Application.Inventario.Handlers;
using PharmaFlow.Application.Inventario.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Persistence.Controllers.Inventario;

[ApiController]
[Route("api/[controller]")]
public class InventarioController : ControllerBase
{
    // Inyectamos los Puertos (Interfaces) que ya creaste. 
    // ASP.NET Core se encargará de entregar los repositorios reales aquí.
    private readonly IStockLoteRepository _stockLoteRepository;
    private readonly IMovimientoInventarioRepository _movimientoRepository;
    private readonly IInventarioReader _inventarioReader;

    public InventarioController(
        IStockLoteRepository stockLoteRepository,
        IMovimientoInventarioRepository movimientoRepository,
        IInventarioReader inventarioReader)
    {
        _stockLoteRepository = stockLoteRepository;
        _movimientoRepository = movimientoRepository;
        _inventarioReader = inventarioReader;
    }

    // ENDPOINT 1: POST /api/inventario/ajustar
    [HttpPost("ajustar")]
    public async Task<IActionResult> AjustarStock([FromBody] AjustarStockRequestDto request, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Armamos el Command
            var command = new AjustarStockCommand(request);
            
            // 2. Instanciamos el Handler pasándole los repositorios
            var handler = new AjustarStockHandler(_stockLoteRepository, _movimientoRepository);

            // 3. Ejecutamos
            var resultado = await handler.EjecutarAsync(command, cancellationToken);
            
            // 4. Retornamos 200 OK con el DTO de respuesta
            return Ok(resultado);
        }
        catch (ArgumentException ex)
        {
            // Si falla una validación manual, devolvemos un error 400 (Bad Request)
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (Exception ex)
        {
            // Si hay otro tipo de error, devolvemos un 500 (Internal Server Error)
            return StatusCode(500, new { mensaje = ex.Message });
        }
    }

    // ENDPOINT 2: GET /api/inventario/sucursal/{idSucursal}
    [HttpGet("sucursal/{idSucursal}")]
    public async Task<IActionResult> ObtenerStockPorSucursal(Guid idSucursal, CancellationToken cancellationToken)
    {
        try
        {
            // 1. Armamos el Query
            var query = new ObtenerStockPorSucursalQuery(idSucursal);
            
            // 2. Instanciamos el Handler
            var handler = new ObtenerStockPorSucursalHandler(_inventarioReader);

            // 3. Ejecutamos
            var resultado = await handler.EjecutarAsync(query, cancellationToken);
            
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