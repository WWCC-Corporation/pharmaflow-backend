using PharmaFlow.Application.Usuarios.Commands;
using PharmaFlow.Application.Usuarios.DTOs;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Usuarios.Handlers;

public class ActualizarUsuarioHandler
{
    private readonly IUsuarioRepository usuarioRepository;

    public ActualizarUsuarioHandler(IUsuarioRepository usuarioRepository)
    {
        this.usuarioRepository = usuarioRepository;
    }

    public async Task<UsuarioDto?> Handle(ActualizarUsuarioCommand command, CancellationToken cancellationToken)
    {
        var usuario = await usuarioRepository.ObtenerPorIdAsync(command.Id, cancellationToken);

        if (usuario is null)
        {
            return null;
        }

        if (command.Correo is not null)
        {
            if (string.IsNullOrWhiteSpace(command.Correo))
            {
                throw new InvalidOperationException("El correo no puede estar vacio.");
            }

            var correo = command.Correo.Trim();
            if (await usuarioRepository.ExisteCorreoAsync(correo, usuario.Id, cancellationToken))
            {
                throw new InvalidOperationException("Ya existe un usuario con ese correo.");
            }

            usuario.Correo = correo;
        }

        if (command.Nombres is not null) usuario.Nombres = string.IsNullOrWhiteSpace(command.Nombres) ? null : command.Nombres.Trim();
        if (command.Apellidos is not null) usuario.Apellidos = string.IsNullOrWhiteSpace(command.Apellidos) ? null : command.Apellidos.Trim();
        if (command.IdRol.HasValue) usuario.IdRol = command.IdRol.Value;
        if (command.Activo.HasValue) usuario.Activo = command.Activo.Value;
        usuario.UpdatedAt = DateTime.UtcNow;

        await usuarioRepository.ActualizarAsync(usuario, command.IdSucursales, cancellationToken);

        return UsuarioMapper.ToDto(usuario);
    }
}
