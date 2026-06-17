using PharmaFlow.Application.Proveedores.DTOs;
using PharmaFlow.Application.Proveedores.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Proveedores.Handlers;

public class ObtenerProveedorPorIdHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public ObtenerProveedorPorIdHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorDto?> Handle(ObtenerProveedorPorIdQuery query, CancellationToken cancellationToken)
    {
        var proveedor = await proveedorRepository.ObtenerPorIdAsync(query.Id, cancellationToken);

        if (proveedor is null)
        {
            return null;
        }

        return new ProveedorDto
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
