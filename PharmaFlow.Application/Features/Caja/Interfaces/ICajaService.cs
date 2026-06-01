using PharmaFlow.Application.Features.Caja.DTOs;
using System;
using System.Threading.Tasks;

namespace PharmaFlow.Application.Features.Caja.Interfaces
{
    public interface ICajaService
    {
        Task<TurnoCajaResponseDto> AbrirCajaAsync(AperturaCajaDto dto);
        Task<TurnoCajaResponseDto> CerrarCajaAsync(CierreCajaDto dto);
        Task<bool> RegistrarMovimientoAsync(RegistrarMovimientoDto dto);
        Task<TurnoCajaResponseDto?> ObtenerEstadoCajaAsync(Guid idUsuario);
    }
}
