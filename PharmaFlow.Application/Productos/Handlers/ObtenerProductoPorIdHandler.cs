using PharmaFlow.Application.Productos.DTOs;
using PharmaFlow.Application.Productos.Queries;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Productos.Handlers;

public class ObtenerProductoPorIdHandler
{
    private readonly IProductoRepository productoRepository;

    public ObtenerProductoPorIdHandler(IProductoRepository productoRepository)
    {
        this.productoRepository = productoRepository;
    }

    public async Task<ProductoDto?> Handle(ObtenerProductoPorIdQuery query, CancellationToken cancellationToken)
    {
        var producto = await productoRepository.ObtenerPorIdAsync(query.Id, cancellationToken);

        return producto is null ? null : ProductoMapper.ToDto(producto);
    }
}
