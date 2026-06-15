using PharmaFlow.Application.Features.Proveedores.Commands;
using PharmaFlow.Application.Features.Proveedores.DTOs;

namespace PharmaFlow.Application.Features.Proveedores.Handlers;

public class ActualizarProveedorHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public ActualizarProveedorHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public Task<ProveedorResponseDto?> Handle(ActualizarProveedorCommand command, CancellationToken cancellationToken)
    {
        var datos = command.Datos;

        if (string.IsNullOrWhiteSpace(datos.Nombre))
        {
            throw new ArgumentException("El nombre del proveedor es obligatorio.");
        }

        return proveedorRepository.ActualizarAsync(command.Id, datos, cancellationToken);
    }
}
