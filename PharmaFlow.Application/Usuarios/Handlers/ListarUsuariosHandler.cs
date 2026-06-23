using PharmaFlow.Application.Usuarios.DTOs;
using PharmaFlow.Application.Usuarios.Queries;

namespace PharmaFlow.Application.Usuarios.Handlers;

public class ListarUsuariosHandler
{
    public Task<List<UsuarioDto>> Handle(ListarUsuariosQuery query)
    {
        return Task.FromResult(new List<UsuarioDto>());
    }
}