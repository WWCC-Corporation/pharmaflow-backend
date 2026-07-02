using PharmaFlow.Application.Sucursales.DTOs;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Sucursales.Handlers;

public static class SucursalMapper
{
    public static SucursalDto ToDto(Sucursale sucursal)
    {
        return new SucursalDto
        {
            Id = sucursal.Id,
            Codigo = sucursal.Codigo,
            Nombre = sucursal.Nombre,
            Direccion = sucursal.Direccion,
            Telefono = sucursal.Telefono,
            Activo = sucursal.Activo
        };
    }
}
