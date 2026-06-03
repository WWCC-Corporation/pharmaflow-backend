namespace PharmaFlow.Application.Features.Auth.DTOs;

public class LoginResponseDto
{
    public string Token { get; set; }
    public string Correo { get; set; }
    public string Rol { get; set; }
}