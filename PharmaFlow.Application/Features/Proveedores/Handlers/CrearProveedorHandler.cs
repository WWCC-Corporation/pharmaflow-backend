using PharmaFlow.Application.Features.Proveedores.Commands;
using PharmaFlow.Application.Features.Proveedores.DTOs;

namespace PharmaFlow.Application.Features.Proveedores.Handlers;

public class CrearProveedorHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public CrearProveedorHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public Task<ProveedorResponseDto> Handle(CrearProveedorCommand command, CancellationToken cancellationToken)
    {
        var datos = command.Datos;

        if (string.IsNullOrWhiteSpace(datos.Nombre))
        {
            throw new ArgumentException("El nombre del proveedor es obligatorio.");
        }

        return proveedorRepository.CrearAsync(datos, cancellationToken);
    }
}
