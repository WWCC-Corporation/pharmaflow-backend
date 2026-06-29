namespace PharmaFlow.Application.Clientes.DTOs;

public class ClienteResponseDto
{
    public Guid Id { get; set; }

    public string? Dni { get; set; }

    public string? Nombres { get; set; }

    public string? Apellidos { get; set; }

    public string? Telefono { get; set; }

    public string? Correo { get; set; }

    public bool? Activo { get; set; }
}
