using PharmaFlow.Application.Clientes.DTOs;

namespace PharmaFlow.Application.Clientes.Commands;

public class ActualizarClienteCommand
{
    public Guid Id { get; set; }

    public UpdateClienteDto Datos { get; set; } = null!;
}
