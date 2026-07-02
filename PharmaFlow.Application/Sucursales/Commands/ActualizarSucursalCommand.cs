namespace PharmaFlow.Application.Sucursales.Commands;

public class ActualizarSucursalCommand
{
    public Guid Id { get; set; }

    public string? Codigo { get; set; }

    public string? Nombre { get; set; }

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }

    public bool? Activo { get; set; }
}
