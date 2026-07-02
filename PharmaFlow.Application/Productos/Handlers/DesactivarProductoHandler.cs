using PharmaFlow.Application.Productos.Commands;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Productos.Handlers;

public class DesactivarProductoHandler
{
    private readonly IProductoRepository productoRepository;

    public DesactivarProductoHandler(IProductoRepository productoRepository)
    {
        this.productoRepository = productoRepository;
    }

    public async Task<bool> Handle(DesactivarProductoCommand command, CancellationToken cancellationToken)
    {
        var producto = await productoRepository.ObtenerPorIdAsync(command.Id, cancellationToken);

        if (producto is null)
        {
            return false;
        }

        producto.Activo = false;
        producto.UpdatedAt = DateTime.UtcNow;

        await productoRepository.ActualizarAsync(producto, cancellationToken);

        return true;
    }
}
