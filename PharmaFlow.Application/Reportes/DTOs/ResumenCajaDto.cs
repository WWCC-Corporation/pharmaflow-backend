namespace PharmaFlow.Application.Reportes.DTOs;

public class ResumenCajaDto
{
    public Guid? IdSucursal { get; set; }

    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }

    public int TotalMovimientos { get; set; }

    public decimal TotalIngresos { get; set; }

    public decimal TotalEgresos { get; set; }

    public decimal Balance { get; set; }

    public IReadOnlyList<MovimientoCajaResumenDto> Movimientos { get; set; } = [];
}
