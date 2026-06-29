namespace PharmaFlow.Application.Usuarios.DTOs;

public class UsuarioDto
{
    public Guid Id { get; set; }

    public string Correo { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public bool Activo { get; set; }
}