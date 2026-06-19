using PharmaFlow.Application.Proveedores.Commands;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Proveedores.Handlers;

public class DesactivarProveedorHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public DesactivarProveedorHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public async Task Handle(DesactivarProveedorCommand command, CancellationToken cancellationToken)
    {
        var proveedor = await proveedorRepository.ObtenerPorIdAsync(command.Id, cancellationToken);

        if (proveedor is null)
        {
            throw new InvalidOperationException($"No existe un proveedor con el id {command.Id}.");
        }

        proveedor.Activo = false;

        await proveedorRepository.ActualizarAsync(proveedor, cancellationToken);
    }
}
