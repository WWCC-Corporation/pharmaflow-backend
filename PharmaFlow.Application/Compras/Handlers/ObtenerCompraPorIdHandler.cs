using PharmaFlow.Application.Compras.DTOs;
using PharmaFlow.Application.Compras.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Compras.Handlers;

public class ObtenerCompraPorIdHandler
{
    private readonly ICompraRepository compraRepository;

    public ObtenerCompraPorIdHandler(ICompraRepository compraRepository)
    {
        this.compraRepository = compraRepository;
    }

    public async Task<CompraDto?> Handle(ObtenerCompraPorIdQuery query, CancellationToken cancellationToken)
    {
        var compra = await compraRepository.ObtenerPorIdAsync(query.Id, cancellationToken);

        if (compra is null)
        {
            return null;
        }

        return CompraMapper.ToDto(compra);
    }
}
