using PharmaFlow.Application.Clientes.DTOs;

namespace PharmaFlow.Application.Clientes.Commands;

public class CrearClienteCommand
{
    public CreateClienteDto Datos { get; set; } = null!;
}
