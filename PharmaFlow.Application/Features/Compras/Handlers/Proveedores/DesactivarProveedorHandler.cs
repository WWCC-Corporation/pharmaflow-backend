using MediatR;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.Commands.Proveedores;

namespace PharmaFlow.Application.Features.Compras.Handlers.Proveedores;

public class DesactivarProveedorHandler : IRequestHandler<DesactivarProveedorCommand, bool>
{
    private readonly IProveedorRepository _proveedorRepository;

    public DesactivarProveedorHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<bool> Handle(DesactivarProveedorCommand request, CancellationToken cancellationToken)
    {
        // Baja lógica del proveedor (se marca Activo = false en el repositorio).
        return await _proveedorRepository.DeleteAsync(request.Id);
    }
}
