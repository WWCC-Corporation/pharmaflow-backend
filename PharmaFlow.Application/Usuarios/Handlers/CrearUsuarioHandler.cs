using PharmaFlow.Application.Usuarios.Commands;

namespace PharmaFlow.Application.Usuarios.Handlers;

public class CrearUsuarioHandler
{
    public Task<bool> Handle(CrearUsuarioCommand command)
    {
        return Task.FromResult(true);
    }
}