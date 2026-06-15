using MediatR;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Application.Features.Compras.Commands.Proveedores;
using PharmaFlow.Application.Features.Compras.DTOs.Proveedores;
using PharmaFlow.Application.Features.Compras.Mappings;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Features.Compras.Handlers.Proveedores;

public class CrearProveedorHandler : IRequestHandler<CrearProveedorCommand, ProveedorResponseDto>
{
    private readonly IProveedorRepository _proveedorRepository;

    public CrearProveedorHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorResponseDto> Handle(CrearProveedorCommand request, CancellationToken cancellationToken)
    {
        var dto = request.Datos;

        var proveedor = new Proveedore
        {
            Id = Guid.NewGuid(),
            Nombre = dto.Nombre.Trim(),
            Ruc = dto.Ruc?.Trim(),
            Telefono = dto.Telefono?.Trim(),
            Correo = dto.Correo?.Trim(),
            Activo = true
        };

        var creado = await _proveedorRepository.CreateAsync(proveedor);

        return ProveedorMapper.ToResponse(creado);
    }
}
