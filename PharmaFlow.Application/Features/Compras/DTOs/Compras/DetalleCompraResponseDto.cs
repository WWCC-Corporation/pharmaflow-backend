namespace PharmaFlow.Application.Features.Compras.DTOs.Compras;

public class DetalleCompraResponseDto
{
    public Guid Id { get; set; }

    public Guid? IdProducto { get; set; }

    public string? NombreProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal Subtotal { get; set; }
}