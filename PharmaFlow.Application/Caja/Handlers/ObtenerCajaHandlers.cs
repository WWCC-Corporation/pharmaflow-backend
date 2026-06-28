using PharmaFlow.Application.Caja.DTOs;
using PharmaFlow.Application.Caja.Mappings;
using PharmaFlow.Application.Caja.Queries;
using PharmaFlow.Domain.Enums;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Caja.Handlers;

public class ObtenerTurnoCajaActualHandler
{
    private readonly ICajaRepository _cajaRepository;

    public ObtenerTurnoCajaActualHandler(ICajaRepository cajaRepository)
    {
        _cajaRepository = cajaRepository;
    }

    public async Task<AperturaCajaDto?> Handle(ObtenerTurnoCajaActualQuery query)
    {
        var turno = await _cajaRepository.GetTurnoAbiertoPorUsuarioAsync(query.IdUsuario);

        if (turno == null) return null;

        return CajaMapper.ToAperturaDto(turno);
    }
}

public class ObtenerDetalleTurnoCajaHandler
{
    private readonly ICajaRepository _cajaRepository;

    public ObtenerDetalleTurnoCajaHandler(ICajaRepository cajaRepository)
    {
        _cajaRepository = cajaRepository;
    }

    public async Task<ResumenCajaDto> Handle(ObtenerDetalleTurnoCajaQuery query)
    {
        var turno = await _cajaRepository.GetTurnoPorIdAsync(query.IdTurnoCaja)
            ?? throw new KeyNotFoundException("El turno de caja no existe.");

        var movimientos = await _cajaRepository.GetMovimientosPorTurnoAsync(query.IdTurnoCaja);

        var totalVentasEfectivo = movimientos
            .Where(m => m.Tipo == TipoMovimientoCaja.VENTA_EFECTIVO_INGRESO)
            .Sum(m => m.Monto);

        var totalIngresosManuales = movimientos
            .Where(m => m.Tipo == TipoMovimientoCaja.INGRESO_MANUAL)
            .Sum(m => m.Monto);

        var totalEgresosManuales = movimientos
            .Where(m => m.Tipo == TipoMovimientoCaja.EGRESO_MANUAL)
            .Sum(m => m.Monto);

        var montoEsperado = turno.MontoApertura
            + totalVentasEfectivo
            + totalIngresosManuales
            - totalEgresosManuales;

        return new ResumenCajaDto
        {
            IdTurnoCaja = turno.Id,
            MontoApertura = turno.MontoApertura,
            TotalIngresosVentas = totalVentasEfectivo,
            TotalIngresosManuales = totalIngresosManuales,
            TotalEgresosManuales = totalEgresosManuales,
            MontoEsperado = montoEsperado,
            Movimientos = movimientos.Select(CajaMapper.ToMovimientoDto).ToList()
        };
    }
}
