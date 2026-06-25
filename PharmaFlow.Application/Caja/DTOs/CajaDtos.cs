using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Caja.DTOs;

public class AperturaCajaDto
{
    public Guid Id { get; set; }
    public Guid? IdUsuario { get; set; }
    public decimal? MontoApertura { get; set; }
    public bool? Abierto { get; set; }
    public DateTime? CreatedAt { get; set; }
}

public class CierreCajaDto
{
    public Guid Id { get; set; }
    public decimal? MontoApertura { get; set; }
    public decimal? MontoVentas { get; set; }
    public decimal? MontoContado { get; set; }
    public decimal? DiferenciaCaja { get; set; }
    public bool? Abierto { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? ClosedAt { get; set; }
}

public class MovimientoCajaDto
{
    public Guid Id { get; set; }
    public Guid? IdTurnoCaja { get; set; }
    public Guid? IdUsuario { get; set; }
    public Guid? IdVenta { get; set; }
    public TipoMovimientoCaja Tipo { get; set; }
    public decimal Monto { get; set; }
    public string? Descripcion { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class ResumenCajaDto
{
    public Guid IdTurnoCaja { get; set; }
    public decimal? MontoApertura { get; set; }
    public decimal TotalIngresosVentas { get; set; }
    public decimal TotalIngresosManuales { get; set; }
    public decimal TotalEgresosManuales { get; set; }
    public decimal MontoEsperado { get; set; }
    public List<MovimientoCajaDto> Movimientos { get; set; } = new();
}
