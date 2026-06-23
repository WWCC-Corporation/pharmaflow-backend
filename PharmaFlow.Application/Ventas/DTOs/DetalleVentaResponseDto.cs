namespace PharmaFlow.Application.Features.Ventas.DTOs;

public class DetalleVentaResponseDto
{
    public Guid Id { get; set; }

    public Guid? IdVenta { get; set; }

    public Guid? IdLote { get; set; }

    public Guid? IdProducto { get; set; }

    public int Cantidad { get; set; }

    public decimal? PrecioUnitario { get; set; }
}
