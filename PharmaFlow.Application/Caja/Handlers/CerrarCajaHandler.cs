using PharmaFlow.Application.Caja.Commands;
using PharmaFlow.Application.Caja.DTOs;
using PharmaFlow.Application.Caja.Mappings;
using PharmaFlow.Application.Caja.Validators;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Caja.Handlers;

public class CerrarCajaHandler
{
    private readonly ICajaRepository _cajaRepository;

    public CerrarCajaHandler(ICajaRepository cajaRepository)
    {
        _cajaRepository = cajaRepository;
    }

    public async Task<CierreCajaDto> Handle(CerrarCajaCommand command)
    {
        CerrarCajaValidator.Validate(command);

        var turno = await _cajaRepository.GetTurnoPorIdAsync(command.IdTurnoCaja)
            ?? throw new KeyNotFoundException("El turno de caja no existe.");

        if (turno.Abierto == false)
            throw new InvalidOperationException("El turno de caja ya se encuentra cerrado.");

        if (turno.IdUsuario != command.IdUsuario)
            throw new InvalidOperationException("El usuario no tiene permiso para cerrar este turno.");

        // Calcular totales desde los movimientos registrados
        var totalVentasEfectivo = await _cajaRepository.SumarMovimientosPorTipoAsync(
            command.IdTurnoCaja, TipoMovimientoCaja.VENTA_EFECTIVO_INGRESO);

        var totalIngresosManuales = await _cajaRepository.SumarMovimientosPorTipoAsync(
            command.IdTurnoCaja, TipoMovimientoCaja.INGRESO_MANUAL);

        var totalEgresosManuales = await _cajaRepository.SumarMovimientosPorTipoAsync(
            command.IdTurnoCaja, TipoMovimientoCaja.EGRESO_MANUAL);

        var totalVueltos = await _cajaRepository.SumarMovimientosPorTipoAsync(
            command.IdTurnoCaja, TipoMovimientoCaja.VUELTO_SALIDA);

        // Monto esperado = Apertura + Ventas Efectivo + Ingresos Manuales - Egresos Manuales - Vueltos
        var montoEsperado = turno.MontoApertura
            + totalVentasEfectivo
            + totalIngresosManuales
            - totalEgresosManuales
            - totalVueltos;

        turno.MontoVentas = totalVentasEfectivo;
        turno.MontoContado = command.MontoContado;
        turno.DiferenciaCaja = command.MontoContado - montoEsperado;
        turno.Abierto = false;
        turno.ClosedAt = DateTime.UtcNow;

        await _cajaRepository.UpdateTurnoCajaAsync(turno);

        return CajaMapper.ToCierreDto(turno);
    }
}
