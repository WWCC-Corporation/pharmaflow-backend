namespace PharmaFlow.Application.Usuarios.Commands;

public class CrearUsuarioCommand
{
    public string Correo { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Nombres { get; set; } = string.Empty;

    public string Apellidos { get; set; } = string.Empty;

    public int IdRol { get; set; } = 3;

    public List<Guid> IdSucursales { get; set; } = new();
}
