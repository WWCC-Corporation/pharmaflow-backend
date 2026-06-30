using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Caja.Commands;
using PharmaFlow.Application.Caja.Handlers;
using PharmaFlow.Application.Caja.Queries;

namespace PharmaFlow.Persistence.Controllers.Caja;

[ApiController]
[Route("api/[controller]")]
public class CajaController : ControllerBase
{
    private readonly AbrirCajaHandler _abrirCajaHandler;
    private readonly CerrarCajaHandler _cerrarCajaHandler;
    private readonly RegistrarMovimientoCajaHandler _registrarMovimientoCajaHandler;
    private readonly ObtenerTurnoCajaActualHandler _obtenerTurnoCajaActualHandler;
    private readonly ObtenerDetalleTurnoCajaHandler _obtenerDetalleTurnoCajaHandler;

    public CajaController(
        AbrirCajaHandler abrirCajaHandler,
        CerrarCajaHandler cerrarCajaHandler,
        RegistrarMovimientoCajaHandler registrarMovimientoCajaHandler,
        ObtenerTurnoCajaActualHandler obtenerTurnoCajaActualHandler,
        ObtenerDetalleTurnoCajaHandler obtenerDetalleTurnoCajaHandler)
    {
        _abrirCajaHandler = abrirCajaHandler;
        _cerrarCajaHandler = cerrarCajaHandler;
        _registrarMovimientoCajaHandler = registrarMovimientoCajaHandler;
        _obtenerTurnoCajaActualHandler = obtenerTurnoCajaActualHandler;
        _obtenerDetalleTurnoCajaHandler = obtenerDetalleTurnoCajaHandler;
    }

    [HttpPost("abrir")]
    public async Task<IActionResult> AbrirCaja([FromBody] AbrirCajaCommand command)
    {
        try
        {
            var result = await _abrirCajaHandler.Handle(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("cerrar")]
    public async Task<IActionResult> CerrarCaja([FromBody] CerrarCajaCommand command)
    {
        try
        {
            var result = await _cerrarCajaHandler.Handle(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpPost("movimiento")]
    public async Task<IActionResult> RegistrarMovimiento([FromBody] RegistrarMovimientoCajaCommand command)
    {
        try
        {
            var result = await _registrarMovimientoCajaHandler.Handle(command);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { Message = ex.Message });
        }
    }

    [HttpGet("estado/{idUsuario}")]
    public async Task<IActionResult> ObtenerEstadoCaja(Guid idUsuario, [FromQuery] Guid idSucursal)
    {
        var query = new ObtenerTurnoCajaActualQuery { IdUsuario = idUsuario, IdSucursal = idSucursal };
        var result = await _obtenerTurnoCajaActualHandler.Handle(query);
        
        if (result == null)
        {
            return NotFound(new { Message = "El usuario no tiene una caja abierta." });
        }
        return Ok(result);
    }

    [HttpGet("resumen/{idTurnoCaja}")]
    public async Task<IActionResult> ObtenerResumenCaja(Guid idTurnoCaja)
    {
        try
        {
            var query = new ObtenerDetalleTurnoCajaQuery { IdTurnoCaja = idTurnoCaja };
            var result = await _obtenerDetalleTurnoCajaHandler.Handle(query);
            return Ok(result);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { Message = ex.Message });
        }
    }
}
