using Microsoft.AspNetCore.Mvc;
using PharmaFlow.Application.Features.Caja.DTOs;
using PharmaFlow.Application.Features.Caja.Interfaces;
using System;
using System.Threading.Tasks;

namespace PharmaFlow.Persistence.Controllers.Caja
{
    [ApiController]
    [Route("api/[controller]")]
    public class CajaController : ControllerBase
    {
        private readonly ICajaService _cajaService;

        public CajaController(ICajaService cajaService)
        {
            _cajaService = cajaService;
        }

        [HttpPost("abrir")]
        public async Task<IActionResult> AbrirCaja([FromBody] AperturaCajaDto dto)
        {
            try
            {
                var result = await _cajaService.AbrirCajaAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("cerrar")]
        public async Task<IActionResult> CerrarCaja([FromBody] CierreCajaDto dto)
        {
            try
            {
                var result = await _cajaService.CerrarCajaAsync(dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPost("movimiento")]
        public async Task<IActionResult> RegistrarMovimiento([FromBody] RegistrarMovimientoDto dto)
        {
            try
            {
                await _cajaService.RegistrarMovimientoAsync(dto);
                return Ok(new { Message = "Movimiento registrado exitosamente." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet("estado/{idUsuario}")]
        public async Task<IActionResult> ObtenerEstadoCaja(Guid idUsuario)
        {
            var result = await _cajaService.ObtenerEstadoCajaAsync(idUsuario);
            if (result == null)
            {
                return NotFound(new { Message = "El usuario no tiene una caja abierta." });
            }
            return Ok(result);
        }
    }
}
