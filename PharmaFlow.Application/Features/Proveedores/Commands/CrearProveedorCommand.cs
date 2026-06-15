using PharmaFlow.Application.Features.Proveedores.DTOs;

namespace PharmaFlow.Application.Features.Proveedores.Commands;

public class CrearProveedorCommand
{
    public CreateProveedorDto Datos { get; set; } = new();
}
