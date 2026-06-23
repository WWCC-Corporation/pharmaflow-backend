using PharmaFlow.Application.Compras.DTOs;
using PharmaFlow.Application.Compras.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Compras.Handlers;

public class ListarComprasHandler
{
    private readonly ICompraRepository compraRepository;

    public ListarComprasHandler(ICompraRepository compraRepository)
    {
        this.compraRepository = compraRepository;
    }

    public async Task<IReadOnlyList<CompraDto>> Handle(ListarComprasQuery query, CancellationToken cancellationToken)
    {
        var compras = await compraRepository.ListarAsync(query.IdSucursal, query.Desde, query.Hasta, cancellationToken);

        return compras
            .Select(CompraMapper.ToDto)
            .ToList();
    }
}
