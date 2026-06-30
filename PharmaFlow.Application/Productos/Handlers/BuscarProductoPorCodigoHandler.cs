using PharmaFlow.Application.Productos.DTOs;
using PharmaFlow.Application.Productos.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Productos.Handlers;

public class BuscarProductoPorCodigoHandler
{
    private readonly IProductoRepository productoRepository;

    public BuscarProductoPorCodigoHandler(IProductoRepository productoRepository)
    {
        this.productoRepository = productoRepository;
    }

    public async Task<ProductoDto?> Handle(BuscarProductoPorCodigoQuery query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query.CodigoBarra))
        {
            throw new InvalidOperationException("El codigo de barras es requerido.");
        }

        var producto = await productoRepository.ObtenerPorCodigoBarraAsync(query.CodigoBarra.Trim(), cancellationToken);

        return producto is null ? null : ProductoMapper.ToDto(producto);
    }
}
