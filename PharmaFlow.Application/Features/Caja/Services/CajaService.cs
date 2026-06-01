using PharmaFlow.Application.Features.Caja.DTOs;
using PharmaFlow.Application.Features.Caja.Interfaces;
using PharmaFlow.Domain.Ports;
using PharmaFlow.Persistence;
using System;
using System.Threading.Tasks;

namespace PharmaFlow.Application.Features.Caja.Services
{
    public class CajaService : ICajaService
    {
        private readonly ICajaRepository _cajaRepository;

        public CajaService(ICajaRepository cajaRepository)
        {
            _cajaRepository = cajaRepository;
        }

        public async Task<TurnoCajaResponseDto> AbrirCajaAsync(AperturaCajaDto dto)
        {
            var turnoAbierto = await _cajaRepository.GetTurnoAbiertoPorUsuarioAsync(dto.IdUsuario);
            if (turnoAbierto != null)
                throw new Exception("El usuario ya tiene una caja abierta.");

            var nuevoTurno = new TurnosCaja
            {
                Id = Guid.NewGuid(),
                IdUsuario = dto.IdUsuario,
                MontoApertura = dto.MontoApertura,
                Abierto = true,
                CreatedAt = DateTime.UtcNow
            };

            await _cajaRepository.AddTurnoCajaAsync(nuevoTurno);
            return MapToResponseDto(nuevoTurno);
        }

        public async Task<TurnoCajaResponseDto> CerrarCajaAsync(CierreCajaDto dto)
        {
            var turno = await _cajaRepository.GetTurnoPorIdAsync(dto.IdTurnoCaja);
            if (turno == null || turno.Abierto == false)
                throw new Exception("El turno de caja no existe o ya está cerrado.");

            var totalVentas = await _cajaRepository.CalcularTotalVentasTurnoAsync(dto.IdTurnoCaja);
            var totalMovimientos = await _cajaRepository.CalcularTotalMovimientosTurnoAsync(dto.IdTurnoCaja);

            turno.MontoVentas = totalVentas;
            turno.MontoContado = dto.MontoContado;
            
            // Calculamos la diferencia: Monto Fisico - (Monto Inicial + Ventas + Ingresos - Egresos)
            var montoEsperado = (turno.MontoApertura ?? 0) + totalVentas + totalMovimientos;
            turno.DiferenciaCaja = dto.MontoContado - montoEsperado;
            
            turno.Abierto = false;
            turno.ClosedAt = DateTime.UtcNow;

            await _cajaRepository.UpdateTurnoCajaAsync(turno);
            return MapToResponseDto(turno);
        }

        public async Task<bool> RegistrarMovimientoAsync(RegistrarMovimientoDto dto)
        {
            var turno = await _cajaRepository.GetTurnoPorIdAsync(dto.IdTurnoCaja);
            if (turno == null || turno.Abierto == false)
                throw new Exception("El turno de caja no existe o está cerrado.");

            var movimiento = new MovimientosCaja
            {
                Id = Guid.NewGuid(),
                IdTurnoCaja = dto.IdTurnoCaja,
                Tipo = dto.Tipo,
                Monto = dto.Monto,
                Descripcion = dto.Descripcion,
                CreatedAt = DateTime.UtcNow,
                IdVenta = dto.IdVenta
            };

            await _cajaRepository.AddMovimientoCajaAsync(movimiento);
            return true;
        }

        public async Task<TurnoCajaResponseDto?> ObtenerEstadoCajaAsync(Guid idUsuario)
        {
            var turno = await _cajaRepository.GetTurnoAbiertoPorUsuarioAsync(idUsuario);
            if (turno == null) return null;
            return MapToResponseDto(turno);
        }

        private TurnoCajaResponseDto MapToResponseDto(TurnosCaja turno)
        {
            return new TurnoCajaResponseDto
            {
                Id = turno.Id,
                IdUsuario = turno.IdUsuario,
                MontoApertura = turno.MontoApertura,
                MontoVentas = turno.MontoVentas,
                MontoContado = turno.MontoContado,
                DiferenciaCaja = turno.DiferenciaCaja,
                Abierto = turno.Abierto,
                CreatedAt = turno.CreatedAt,
                ClosedAt = turno.ClosedAt
            };
        }
    }
}
