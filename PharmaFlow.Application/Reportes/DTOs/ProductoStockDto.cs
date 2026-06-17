namespace PharmaFlow.Application.Reportes.DTOs;

public class ProductoStockDto
{
    public Guid? IdSucursal { get; set; }

    public string? Sucursal { get; set; }

    public Guid? IdProducto { get; set; }

    public string? Producto { get; set; }

    public string? CodigoBarra { get; set; }

    public int? StockMinimo { get; set; }

    public long? StockTotal { get; set; }

    public string? Estado { get; set; }
}
