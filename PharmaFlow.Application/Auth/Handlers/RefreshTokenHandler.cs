using PharmaFlow.Application.Auth.Commands;

namespace PharmaFlow.Application.Auth.Handlers;

public class RefreshTokenHandler
{
    public Task<bool> Handle(RefreshTokenCommand command)
    {
        return Task.FromResult(true);
    }
}