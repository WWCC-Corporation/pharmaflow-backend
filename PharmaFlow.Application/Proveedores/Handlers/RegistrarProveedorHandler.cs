using PharmaFlow.Application.Proveedores.Commands;
using PharmaFlow.Application.Proveedores.DTOs;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Proveedores.Handlers;

public class RegistrarProveedorHandler
{
    private readonly IProveedorRepository proveedorRepository;

    public RegistrarProveedorHandler(IProveedorRepository proveedorRepository)
    {
        this.proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorDto> Handle(RegistrarProveedorCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Nombre))
        {
            throw new InvalidOperationException("El nombre del proveedor es obligatorio.");
        }

        if (!string.IsNullOrWhiteSpace(command.Ruc) &&
            await proveedorRepository.ExisteRucAsync(command.Ruc, null, cancellationToken))
        {
            throw new InvalidOperationException($"Ya existe un proveedor con el RUC {command.Ruc}.");
        }

        var proveedor = new Proveedore
        {
            Nombre = command.Nombre.Trim(),
            Ruc = command.Ruc,
            Telefono = command.Telefono,
            Correo = command.Correo,
            Activo = true
        };

        await proveedorRepository.AgregarAsync(proveedor, cancellationToken);

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
