namespace PharmaFlow.Application.Compras.DTOs;

public class CompraDto
{
    public Guid Id { get; set; }

    public Guid IdSucursal { get; set; }

    public Guid? IdProveedor { get; set; }

    public Guid? IdUsuario { get; set; }

    public DateTime Fecha { get; set; }

    public string Estado { get; set; } = null!;

    public string Moneda { get; set; } = null!;

    public decimal TipoCambio { get; set; }

    public IReadOnlyList<DetalleCompraDto> Detalles { get; set; } = new List<DetalleCompraDto>();
}
