namespace PharmaFlow.Application.Features.Auth.Commands;

public class LoginCommand : IRequest<LoginResponseDto>
{
    public string Correo { get; set; }
    public string Password { get; set; }
}