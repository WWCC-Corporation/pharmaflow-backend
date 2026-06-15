using MediatR;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;
using PharmaFlow.Application.Features.Compras.Mappings;
using PharmaFlow.Application.Features.Compras.Queries.Proveedores;

namespace PharmaFlow.Application.Features.Compras.Handlers.Proveedores;

public class ObtenerProveedorPorIdHandler : IRequestHandler<ObtenerProveedorPorIdQuery, ProveedorResponseDto?>
{
    private readonly IProveedorRepository _proveedorRepository;

    public ObtenerProveedorPorIdHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorResponseDto?> Handle(ObtenerProveedorPorIdQuery request, CancellationToken cancellationToken)
    {
        var proveedor = await _proveedorRepository.GetByIdAsync(request.Id);

        return proveedor is null ? null : ProveedorMapper.ToResponse(proveedor);
    }
}
