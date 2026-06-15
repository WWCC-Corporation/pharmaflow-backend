using MediatR;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;
using PharmaFlow.Application.Features.Compras.Mappings;
using PharmaFlow.Application.Features.Compras.Queries.Proveedores;

namespace PharmaFlow.Application.Features.Compras.Handlers.Proveedores;

public class ListarProveedoresHandler : IRequestHandler<ListarProveedoresQuery, List<ProveedorResponseDto>>
{
    private readonly IProveedorRepository _proveedorRepository;

    public ListarProveedoresHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<List<ProveedorResponseDto>> Handle(ListarProveedoresQuery request, CancellationToken cancellationToken)
    {
        var proveedores = await _proveedorRepository.GetAllAsync();

        return proveedores.Select(ProveedorMapper.ToResponse).ToList();
    }
}
