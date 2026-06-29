namespace PharmaFlow.Application.Proveedores.DTOs;

public class ProveedorDto
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Ruc { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public bool Activo { get; set; }
}
