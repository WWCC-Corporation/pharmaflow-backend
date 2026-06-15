using PharmaFlow.Application.Features.Proveedores.Commands;

namespace PharmaFlow.Application.Features.Proveedores.Handlers;

public class DesactivarProveedorHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public DesactivarProveedorHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public Task<bool> Handle(DesactivarProveedorCommand command, CancellationToken cancellationToken)
    {
        return proveedorRepository.DesactivarAsync(command.Id, cancellationToken);
    }
}
