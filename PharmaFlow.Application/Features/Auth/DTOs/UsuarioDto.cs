namespace PharmaFlow.Application.Features.Auth.DTOs;

public class UsuarioDto
{
    public Guid Id { get; set; }
    public string Correo { get; set; }
    public string Nombres { get; set; }
    public string Apellidos { get; set; }
    public string Rol { get; set; }
}