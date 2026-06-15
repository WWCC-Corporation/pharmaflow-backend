using PharmaFlow.Application.Features.Proveedores.DTOs;

namespace PharmaFlow.Application.Features.Proveedores.Commands;

public class ActualizarProveedorCommand
{
    public Guid Id { get; set; }

    public UpdateProveedorDto Datos { get; set; } = new();
}
