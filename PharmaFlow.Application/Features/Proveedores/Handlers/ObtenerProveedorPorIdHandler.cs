using PharmaFlow.Application.Features.Proveedores.DTOs;
using PharmaFlow.Application.Features.Proveedores.Queries;

namespace PharmaFlow.Application.Features.Proveedores.Handlers;

public class ObtenerProveedorPorIdHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public ObtenerProveedorPorIdHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public Task<ProveedorResponseDto?> Handle(ObtenerProveedorPorIdQuery query, CancellationToken cancellationToken)
    {
        return proveedorRepository.ObtenerPorIdAsync(query.Id, cancellationToken);
    }
}
