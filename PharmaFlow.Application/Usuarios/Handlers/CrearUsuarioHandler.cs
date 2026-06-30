using PharmaFlow.Application.Usuarios.Commands;
using PharmaFlow.Application.Usuarios.DTOs;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Domain.Ports.Services;

namespace PharmaFlow.Application.Usuarios.Handlers;

public class CrearUsuarioHandler
{
    private readonly IUsuarioRepository usuarioRepository;
    private readonly IPasswordHasherService passwordHasherService;

    public CrearUsuarioHandler(IUsuarioRepository usuarioRepository, IPasswordHasherService passwordHasherService)
    {
        this.usuarioRepository = usuarioRepository;
        this.passwordHasherService = passwordHasherService;
    }

    public async Task<UsuarioDto> Handle(CrearUsuarioCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Correo))
        {
            throw new InvalidOperationException("El correo es requerido.");
        }

        if (string.IsNullOrWhiteSpace(command.Password))
        {
            throw new InvalidOperationException("La contrasena es requerida.");
        }

        if (await usuarioRepository.ExisteCorreoAsync(command.Correo.Trim(), null, cancellationToken))
        {
            throw new InvalidOperationException("Ya existe un usuario con ese correo.");
        }

        var usuario = new Usuario
        {
            Correo = command.Correo.Trim(),
            PasswordHash = passwordHasherService.Hash(command.Password),
            Nombres = string.IsNullOrWhiteSpace(command.Nombres) ? null : command.Nombres.Trim(),
            Apellidos = string.IsNullOrWhiteSpace(command.Apellidos) ? null : command.Apellidos.Trim(),
            IdRol = command.IdRol,
            Activo = true
        };

        await usuarioRepository.AgregarAsync(usuario, command.IdSucursales, cancellationToken);

        return UsuarioMapper.ToDto(usuario);
    }
}
