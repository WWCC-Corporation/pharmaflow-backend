using PharmaFlow.Application.Productos.DTOs;
using PharmaFlow.Application.Productos.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Productos.Handlers;

public class ListarProductosHandler
{
    private readonly IProductoRepository productoRepository;

    public ListarProductosHandler(IProductoRepository productoRepository)
    {
        this.productoRepository = productoRepository;
    }

    public async Task<IReadOnlyList<ProductoDto>> Handle(ListarProductosQuery query, CancellationToken cancellationToken)
    {
        var productos = await productoRepository.ListarAsync(query.Busqueda, query.SoloActivos, cancellationToken);

        return productos.Select(ProductoMapper.ToDto).ToList();
    }
}
