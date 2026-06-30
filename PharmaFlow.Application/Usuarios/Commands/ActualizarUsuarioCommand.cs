namespace PharmaFlow.Application.Usuarios.Commands;

public class ActualizarUsuarioCommand
{
    public Guid Id { get; set; }

    public string? Correo { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public int? IdRol { get; set; }

    public bool? Activo { get; set; }

    public List<Guid>? IdSucursales { get; set; }
}
