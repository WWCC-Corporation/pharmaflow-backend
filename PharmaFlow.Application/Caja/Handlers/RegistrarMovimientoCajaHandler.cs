using PharmaFlow.Application.Caja.Commands;
using PharmaFlow.Application.Caja.DTOs;
using PharmaFlow.Application.Caja.Mappings;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Caja.Handlers;

public class RegistrarMovimientoCajaHandler
{
    private readonly ICajaRepository _cajaRepository;

    public RegistrarMovimientoCajaHandler(ICajaRepository cajaRepository)
    {
        _cajaRepository = cajaRepository;
    }

    public async Task<MovimientoCajaDto> Handle(RegistrarMovimientoCajaCommand command)
    {
        if (command.Monto <= 0)
            throw new ArgumentException("El monto del movimiento debe ser mayor a cero.");

        // Solo se permiten tipos manuales desde este endpoint
        var tiposPermitidos = new[]
        {
            TipoMovimientoCaja.INGRESO_MANUAL,
            TipoMovimientoCaja.EGRESO_MANUAL
        };

        if (!tiposPermitidos.Contains(command.Tipo))
            throw new ArgumentException("Solo se permiten movimientos de tipo INGRESO_MANUAL o EGRESO_MANUAL desde este endpoint.");

        var turno = await _cajaRepository.GetTurnoPorIdAsync(command.IdTurnoCaja)
            ?? throw new KeyNotFoundException("El turno de caja no existe.");

        if (turno.Abierto == false)
            throw new InvalidOperationException("No se puede registrar un movimiento en un turno cerrado.");

        var movimiento = new MovimientosCaja
        {
            Id = Guid.NewGuid(),
            IdTurnoCaja = command.IdTurnoCaja,
            IdUsuario = command.IdUsuario,
            IdVenta = command.IdVenta,
            Tipo = command.Tipo,
            Monto = command.Monto,
            Descripcion = command.Descripcion,
            CreatedAt = DateTime.UtcNow
        };

        await _cajaRepository.AddMovimientoCajaAsync(movimiento);

        return CajaMapper.ToMovimientoDto(movimiento);
    }
}
