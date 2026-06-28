using PharmaFlow.Application.Caja.DTOs;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Caja.Mappings;

public static class CajaMapper
{
    public static AperturaCajaDto ToAperturaDto(TurnosCaja turno)
    {
        return new AperturaCajaDto
        {
            Id = turno.Id,
            IdUsuario = turno.IdUsuario,
            MontoApertura = turno.MontoApertura,
            Abierto = turno.Abierto,
            CreatedAt = turno.CreatedAt
        };
    }

    public static CierreCajaDto ToCierreDto(TurnosCaja turno)
    {
        return new CierreCajaDto
        {
            Id = turno.Id,
            MontoApertura = turno.MontoApertura,
            MontoVentas = turno.MontoVentas,
            MontoContado = turno.MontoContado,
            DiferenciaCaja = turno.DiferenciaCaja,
            Abierto = turno.Abierto,
            CreatedAt = turno.CreatedAt,
            ClosedAt = turno.ClosedAt
        };
    }

    public static MovimientoCajaDto ToMovimientoDto(MovimientosCaja movimiento)
    {
        return new MovimientoCajaDto
        {
            Id = movimiento.Id,
            IdTurnoCaja = movimiento.IdTurnoCaja,
            IdUsuario = movimiento.IdUsuario,
            IdVenta = movimiento.IdVenta,
            Tipo = movimiento.Tipo,
            Monto = movimiento.Monto,
            Descripcion = movimiento.Descripcion,
            CreatedAt = movimiento.CreatedAt
        };
    }
}
