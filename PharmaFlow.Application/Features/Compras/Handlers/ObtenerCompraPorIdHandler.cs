using PharmaFlow.Application.Features.Compras.DTOs;
using PharmaFlow.Application.Features.Compras.Queries;

namespace PharmaFlow.Application.Features.Compras.Handlers;

public class ObtenerCompraPorIdHandler
{
    private readonly ICompraRepository compraRepository;

    public ObtenerCompraPorIdHandler(ICompraRepository compraRepository)
    {
        this.compraRepository = compraRepository;
    }

    public Task<CompraResponseDto?> Handle(ObtenerCompraPorIdQuery query, CancellationToken cancellationToken)
    {
        return compraRepository.ObtenerPorIdAsync(query.Id, cancellationToken);
    }
}
