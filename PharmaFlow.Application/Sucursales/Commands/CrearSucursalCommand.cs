namespace PharmaFlow.Application.Sucursales.Commands;

public class CrearSucursalCommand
{
    public string Codigo { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string? Direccion { get; set; }

    public string? Telefono { get; set; }
}
