namespace PharmaFlow.Application.Features.Compras.DTOs;

public class CreateDetalleCompraDto
{
    public Guid IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioUnitario { get; set; }

    public string NumeroLote { get; set; } = string.Empty;

    public DateOnly FechaVencimiento { get; set; }
}
