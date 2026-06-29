using PharmaFlow.Application.Proveedores.DTOs;
using PharmaFlow.Application.Proveedores.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Proveedores.Handlers;

public class ListarProveedoresHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public ListarProveedoresHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public async Task<IReadOnlyList<ProveedorDto>> Handle(ListarProveedoresQuery query, CancellationToken cancellationToken)
    {
        var proveedores = await proveedorRepository.ListarAsync(query.SoloActivos, cancellationToken);

        return proveedores
            .Select(proveedor => new ProveedorDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                Ruc = proveedor.Ruc,
                Telefono = proveedor.Telefono,
                Correo = proveedor.Correo,
                Activo = proveedor.Activo
            })
            .ToList();
    }
}
