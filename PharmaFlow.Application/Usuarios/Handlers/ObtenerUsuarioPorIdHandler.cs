using PharmaFlow.Application.Usuarios.DTOs;
using PharmaFlow.Application.Usuarios.Queries;

namespace PharmaFlow.Application.Usuarios.Handlers;

public class ObtenerUsuarioPorIdHandler
{
    public Task<UsuarioDto?> Handle(ObtenerUsuarioPorIdQuery query)
    {
        return Task.FromResult<UsuarioDto?>(null);
    }
}