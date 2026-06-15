using PharmaFlow.Application.Features.Proveedores.DTOs;
using PharmaFlow.Application.Features.Proveedores.Queries;

namespace PharmaFlow.Application.Features.Proveedores.Handlers;

public class ListarProveedoresHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public ListarProveedoresHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public Task<List<ProveedorResponseDto>> Handle(ListarProveedoresQuery query, CancellationToken cancellationToken)
    {
        return proveedorRepository.ListarAsync(cancellationToken);
    }
}
