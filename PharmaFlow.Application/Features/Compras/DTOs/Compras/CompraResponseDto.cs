namespace PharmaFlow.Application.Features.Compras.DTOs.Compras;

public class CompraResponseDto
{
    public Guid Id { get; set; }

    public Guid? IdProveedor { get; set; }

    public string? NombreProveedor { get; set; }

    public Guid? IdUsuario { get; set; }

    public DateTime? Fecha { get; set; }

    public string? Estado { get; set; }

    public string? Moneda { get; set; }

    public decimal? TipoCambio { get; set; }

    public decimal Total { get; set; }

    public List<DetalleCompraResponseDto> Detalles { get; set; } = new();
}