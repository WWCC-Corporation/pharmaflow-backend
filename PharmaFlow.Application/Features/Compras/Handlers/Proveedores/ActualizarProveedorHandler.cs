using MediatR;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.Commands.Proveedores;
using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;
using PharmaFlow.Application.Features.Compras.Mappings;

namespace PharmaFlow.Application.Features.Compras.Handlers.Proveedores;

public class ActualizarProveedorHandler : IRequestHandler<ActualizarProveedorCommand, ProveedorResponseDto?>
{
    private readonly IProveedorRepository _proveedorRepository;

    public ActualizarProveedorHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorResponseDto?> Handle(ActualizarProveedorCommand request, CancellationToken cancellationToken)
    {
        var proveedor = await _proveedorRepository.GetByIdAsync(request.Id);

        if (proveedor is null)
        {
            return null;
        }

        var dto = request.Datos;

        proveedor.Nombre = dto.Nombre.Trim();
        proveedor.Ruc = dto.Ruc?.Trim();
        proveedor.Telefono = dto.Telefono?.Trim();
        proveedor.Correo = dto.Correo?.Trim();
        proveedor.Activo = dto.Activo ?? proveedor.Activo;

        var actualizado = await _proveedorRepository.UpdateAsync(proveedor);

        return actualizado is null ? null : ProveedorMapper.ToResponse(actualizado);
    }
}
