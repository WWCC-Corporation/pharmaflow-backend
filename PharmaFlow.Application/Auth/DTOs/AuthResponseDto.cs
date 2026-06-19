namespace PharmaFlow.Application.Auth.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = string.Empty;

    public Guid UsuarioId { get; set; }

    public string Correo { get; set; } = string.Empty;

    public string Rol { get; set; } = string.Empty;
}