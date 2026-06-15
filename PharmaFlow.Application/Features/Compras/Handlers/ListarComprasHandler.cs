using PharmaFlow.Application.Features.Compras.DTOs;
using PharmaFlow.Application.Features.Compras.Queries;

namespace PharmaFlow.Application.Features.Compras.Handlers;

public class ListarComprasHandler
{
    private readonly ICompraRepository compraRepository;

    public ListarComprasHandler(ICompraRepository compraRepository)
    {
        this.compraRepository = compraRepository;
    }

    public Task<List<CompraResponseDto>> Handle(ListarComprasQuery query, CancellationToken cancellationToken)
    {
        return compraRepository.ListarAsync(cancellationToken);
    }
}
