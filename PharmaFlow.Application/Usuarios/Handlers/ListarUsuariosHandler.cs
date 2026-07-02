using PharmaFlow.Application.Usuarios.DTOs;
using PharmaFlow.Application.Usuarios.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Usuarios.Handlers;

public class ListarUsuariosHandler
{
    private readonly IUsuarioRepository usuarioRepository;

    public ListarUsuariosHandler(IUsuarioRepository usuarioRepository)
    {
        this.usuarioRepository = usuarioRepository;
    }

    public async Task<IReadOnlyList<UsuarioDto>> Handle(ListarUsuariosQuery query, CancellationToken cancellationToken)
    {
        var usuarios = await usuarioRepository.ListarAsync(cancellationToken);

        return usuarios.Select(UsuarioMapper.ToDto).ToList();
    }
}
