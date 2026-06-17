namespace PharmaFlow.Application.Reportes.DTOs;

public class ProductoVencimientoDto
{
    public Guid? IdSucursal { get; set; }

    public string? Sucursal { get; set; }

    public Guid? IdProducto { get; set; }

    public string? Producto { get; set; }

    public string? NumeroLote { get; set; }

    public DateOnly? FechaVencimiento { get; set; }

    public int? StockActual { get; set; }

    public string? Estado { get; set; }
}
