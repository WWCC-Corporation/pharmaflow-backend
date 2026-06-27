namespace PharmaFlow.Application.Compras.DTOs;

public class DetalleCompraDto
{
    public Guid Id { get; set; }

    public Guid IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }
}
