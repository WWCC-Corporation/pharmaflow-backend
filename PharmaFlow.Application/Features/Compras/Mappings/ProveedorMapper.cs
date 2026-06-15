using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Features.Compras.Mappings;

/// <summary>
/// Conversiones entre la entidad Proveedore y sus DTOs de respuesta.
/// </summary>
public static class ProveedorMapper
{
    public static ProveedorResponseDto ToResponse(Proveedore proveedor)
    {
        return new ProveedorResponseDto
        {
            Id = proveedor.Id,
            Nombre = proveedor.Nombre,
            Ruc = proveedor.Ruc,
            Telefono = proveedor.Telefono,
            Correo = proveedor.Correo,
            Activo = proveedor.Activo
        };
    }
}
