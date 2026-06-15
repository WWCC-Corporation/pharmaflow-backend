using MediatR;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.DTOs.Compras;
using PharmaFlow.Application.Features.Compras.Mappings;
using PharmaFlow.Application.Features.Compras.Queries.Compras;

namespace PharmaFlow.Application.Features.Compras.Handlers.Compras;

public class ObtenerCompraPorIdHandler : IRequestHandler<ObtenerCompraPorIdQuery, CompraResponseDto?>
{
    private readonly ICompraRepository _compraRepository;

    public ObtenerCompraPorIdHandler(ICompraRepository compraRepository)
    {
        _compraRepository = compraRepository;
    }

    public async Task<CompraResponseDto?> Handle(ObtenerCompraPorIdQuery request, CancellationToken cancellationToken)
    {
        var compra = await _compraRepository.GetByIdAsync(request.Id);

        return compra is null ? null : CompraMapper.ToResponse(compra);
    }
}
