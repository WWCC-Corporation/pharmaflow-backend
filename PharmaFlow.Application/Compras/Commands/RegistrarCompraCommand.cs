using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Compras.Commands;

public class RegistrarCompraCommand
{
    public Guid IdSucursal { get; set; }

    public Guid? IdProveedor { get; set; }

    public Guid? IdUsuario { get; set; }

    public Moneda Moneda { get; set; }

    public decimal TipoCambio { get; set; }

    public List<DetalleCompraInput> Detalles { get; set; } = new();
}
