namespace PharmaFlow.Application.Reportes.DTOs;

public class ResumenVentasDto
{
    public DateTime? Desde { get; set; }

    public DateTime? Hasta { get; set; }

    public Guid? IdSucursal { get; set; }

    public int TotalVentas { get; set; }

    public decimal TotalVendido { get; set; }

    public decimal PromedioVenta { get; set; }

    public int VentasAnuladas { get; set; }

    public List<VentaPorDiaDto> VentasPorDia { get; set; } = [];
}

public class VentaPorDiaDto
{
    public DateTime Fecha { get; set; }

    public int CantidadVentas { get; set; }

    public decimal TotalVendido { get; set; }
}
