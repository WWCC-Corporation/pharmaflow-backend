namespace PharmaFlow.Application.Features.Compras.DTOs;

public class CompraResponseDto
{
    public Guid Id { get; set; }

    public Guid IdSucursal { get; set; }

    public Guid? IdProveedor { get; set; }

    public string? NombreProveedor { get; set; }

    public Guid? IdUsuario { get; set; }

    public DateTime Fecha { get; set; }

    public string Estado { get; set; } = string.Empty;

    public string Moneda { get; set; } = string.Empty;

    public decimal TipoCambio { get; set; }

    public decimal Total { get; set; }

    public List<DetalleCompraResponseDto> Detalles { get; set; } = new();
}
