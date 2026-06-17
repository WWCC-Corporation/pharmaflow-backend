using PharmaFlow.Application.Proveedores.Commands;
using PharmaFlow.Application.Proveedores.DTOs;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Proveedores.Handlers;

public class ActualizarProveedorHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public ActualizarProveedorHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorDto> Handle(ActualizarProveedorCommand command, CancellationToken cancellationToken)
    {
        var proveedor = await proveedorRepository.ObtenerPorIdAsync(command.Id, cancellationToken);

        if (proveedor is null)
        {
            throw new InvalidOperationException($"No existe un proveedor con el id {command.Id}.");
        }

        if (string.IsNullOrWhiteSpace(command.Nombre))
        {
            throw new InvalidOperationException("El nombre del proveedor es obligatorio.");
        }

        if (!string.IsNullOrWhiteSpace(command.Ruc) &&
            await proveedorRepository.ExisteRucAsync(command.Ruc, command.Id, cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe otro proveedor con el RUC {command.Ruc}.");
        }

        proveedor.Nombre = command.Nombre.Trim();
        proveedor.Ruc = command.Ruc;
        proveedor.Telefono = command.Telefono;
        proveedor.Correo = command.Correo;

        await proveedorRepository.ActualizarAsync(proveedor, cancellationToken);

        return new ProveedorDto
        {
            Id = proveedor.Id,
            Nombre = proveedor.Nombre,
            Ruc = proveedor.Ruc,
            Telefono = proveedor.Telefono,
            Correo = proveedor.Correo,
            Activo = proveedor.Activo
        };
    }
}
