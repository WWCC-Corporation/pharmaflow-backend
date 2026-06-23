using PharmaFlow.Application.Usuarios.Commands;

namespace PharmaFlow.Application.Usuarios.Handlers;

public class ActualizarUsuarioHandler
{
    public Task<bool> Handle(ActualizarUsuarioCommand command)
    {
        return Task.FromResult(true);
    }
}