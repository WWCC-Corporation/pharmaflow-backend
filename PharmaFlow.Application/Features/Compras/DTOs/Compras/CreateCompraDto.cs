using System.ComponentModel.DataAnnotations;
using PharmaFlow.Domain.Enums;

namespace PharmaFlow.Application.Features.Compras.DTOs.Compras;

public class CreateCompraDto
{
    [Required(ErrorMessage = "El proveedor es obligatorio.")]
    public Guid IdProveedor { get; set; }

    public Guid? IdUsuario { get; set; }

    public Moneda? Moneda { get; set; }

    public decimal? TipoCambio { get; set; }

    [MinLength(1, ErrorMessage = "La compra debe tener al menos un producto.")]
    public List<CreateDetalleCompraDto> Detalles { get; set; } = new();
}