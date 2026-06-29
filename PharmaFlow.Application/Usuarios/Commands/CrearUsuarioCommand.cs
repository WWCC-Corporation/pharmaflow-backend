namespace PharmaFlow.Application.Usuarios.Commands;

public class CrearUsuarioCommand
{
    public string Correo { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;
}