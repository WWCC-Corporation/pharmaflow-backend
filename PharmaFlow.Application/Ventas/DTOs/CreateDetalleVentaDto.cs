namespace PharmaFlow.Application.Ventas.DTOs;

public class CreateDetalleVentaDto
{
    public Guid? IdLote { get; set; }

    public Guid? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }
}
