using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Features.Compras.DTOs;

public class CreateCompraDto
{
    public Guid IdSucursal { get; set; }

    public Guid IdProveedor { get; set; }

    public Guid? IdUsuario { get; set; }

    public Moneda Moneda { get; set; } = Moneda.PEN;

    public decimal TipoCambio { get; set; } = 1m;

    public List<CreateDetalleCompraDto> Detalles { get; set; } = new();
}
