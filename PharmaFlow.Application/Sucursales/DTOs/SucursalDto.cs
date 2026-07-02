namespace PharmaFlow.Application.Sucursales.DTOs;

public class SucursalDto
{
    public Guid Id { get; set; }

    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public bool Activo { get; set; }
}
