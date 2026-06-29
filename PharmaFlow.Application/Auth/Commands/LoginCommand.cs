using PharmaFlow.Application.Auth.DTOs;

namespace PharmaFlow.Application.Auth.Commands;

public class LoginCommand
{
    public LoginRequestDto Request { get; set; } = new();
}