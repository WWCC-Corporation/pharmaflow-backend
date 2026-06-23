namespace PharmaFlow.Application.Compras.Commands;

public class DetalleCompraInput
{
    public Guid IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }
}
