namespace PharmaFlow.Application.Features.Proveedores.DTOs;

public class CreateProveedorDto
{
    public string Nombre { get; set; } = string.Empty;

    public string? Ruc { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }
}
