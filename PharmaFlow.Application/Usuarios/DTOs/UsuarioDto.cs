namespace PharmaFlow.Application.Usuarios.DTOs;

public class UsuarioDto
{
    public Guid Id { get; set; }

    public string Correo { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public int? IdRol { get; set; }

    public string? Rol { get; set; }

    public bool Activo { get; set; }

    public List<Guid> IdSucursales { get; set; } = new();
}
