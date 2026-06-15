using MediatR;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.DTOs.Compras;
using PharmaFlow.Application.Features.Compras.Mappings;
using PharmaFlow.Application.Features.Compras.Queries.Compras;

namespace PharmaFlow.Application.Features.Compras.Handlers.Compras;

public class ListarComprasHandler : IRequestHandler<ListarComprasQuery, List<CompraResponseDto>>
{
    private readonly ICompraRepository _compraRepository;

    public ListarComprasHandler(ICompraRepository compraRepository)
    {
        _compraRepository = compraRepository;
    }

    public async Task<List<CompraResponseDto>> Handle(ListarComprasQuery request, CancellationToken cancellationToken)
    {
        var compras = await _compraRepository.GetAllAsync();

        return compras.Select(CompraMapper.ToResponse).ToList();
    }
}
