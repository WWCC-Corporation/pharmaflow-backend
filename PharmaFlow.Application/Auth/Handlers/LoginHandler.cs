using PharmaFlow.Application.Auth.Commands;
using PharmaFlow.Application.Auth.DTOs;

namespace PharmaFlow.Application.Auth.Handlers;

public class LoginHandler
{
    public Task<AuthResponseDto?> Handle(LoginCommand command)
    {
        return Task.FromResult<AuthResponseDto?>(null);
    }
}