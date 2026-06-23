using PharmaFlow.Application.Features.Clientes.DTOs;

namespace PharmaFlow.Application.Features.Clientes.Commands;

public class CrearClienteCommand
{
    public CreateClienteDto Datos { get; set; } = null!;
}
