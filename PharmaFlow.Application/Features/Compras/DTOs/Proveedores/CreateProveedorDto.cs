using System.ComponentModel.DataAnnotations;

namespace PharmaFlow.Application.Features.Compras.DTOs.Proveedores;

public class CreateProveedorDto
{
    [Required(ErrorMessage = "El nombre del proveedor es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(20, ErrorMessage = "El RUC no puede superar los 20 caracteres.")]
    public string? Ruc { get; set; }

    [StringLength(20, ErrorMessage = "El teléfono no puede superar los 20 caracteres.")]
    public string? Telefono { get; set; }

    [EmailAddress(ErrorMessage = "El correo no tiene un formato válido.")]
    [StringLength(150, ErrorMessage = "El correo no puede superar los 150 caracteres.")]
    public string? Correo { get; set; }
}