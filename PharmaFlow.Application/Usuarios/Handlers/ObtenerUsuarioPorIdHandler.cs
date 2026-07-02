using PharmaFlow.Application.Usuarios.DTOs;
using PharmaFlow.Application.Usuarios.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Usuarios.Handlers;

public class ObtenerUsuarioPorIdHandler
{
    private readonly IUsuarioRepository usuarioRepository;

    public ObtenerUsuarioPorIdHandler(IUsuarioRepository usuarioRepository)
    {
        this.usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioDto?> Handle(ObtenerUsuarioPorIdQuery query, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.ObtenerPorIdAsync(query.Id, cancellationToken);

        return usuario is null ? null : UsuarioMapper.ToDto(usuario);
    }
}
