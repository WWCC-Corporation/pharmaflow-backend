using PharmaFlow.Application.Usuarios.DTOs;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Usuarios.Handlers;

public static class UsuarioMapper
{
    public static UsuarioDto ToDto(Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Correo = usuario.Correo,
            Nombres = usuario.Nombres ?? string.Empty,
            Apellidos = usuario.Apellidos ?? string.Empty,
            IdRol = usuario.IdRol,
            Rol = usuario.IdRolNavigation?.Nombre,
            Activo = usuario.Activo,
            IdSucursales = usuario.UsuarioSucursales
                .Where(sucursal => sucursal.Activo)
                .Select(sucursal => sucursal.IdSucursal)
                .ToList()
        };
    }
}
