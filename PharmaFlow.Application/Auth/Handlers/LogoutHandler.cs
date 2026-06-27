using PharmaFlow.Application.Auth.Commands;

namespace PharmaFlow.Application.Auth.Handlers;

public class LogoutHandler
{
    public Task<bool> Handle(LogoutCommand command)
    {
        return Task.FromResult(true);
    }
}