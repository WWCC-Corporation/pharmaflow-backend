using System.ComponentModel.DataAnnotations;

namespace PharmaFlow.Application.Features.Compras.DTOs.Compras;

public class CreateDetalleCompraDto
{
    [Required(ErrorMessage = "El producto es obligatorio.")]
    public Guid IdProducto { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a cero.")]
    public int Cantidad { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a cero.")]
    public decimal PrecioUnitario { get; set; }

    [Required(ErrorMessage = "El número de lote es obligatorio.")]
    [StringLength(50, ErrorMessage = "El número de lote no puede superar los 50 caracteres.")]
    public string NumeroLote { get; set; } = string.Empty;

    [Required(ErrorMessage = "La fecha de vencimiento es obligatoria.")]
    public DateOnly FechaVencimiento { get; set; }
}