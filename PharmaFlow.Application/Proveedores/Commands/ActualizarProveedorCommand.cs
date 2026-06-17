namespace PharmaFlow.Application.Proveedores.Commands;

public class ActualizarProveedorCommand
{
    public Guid Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Ruc { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }
}
