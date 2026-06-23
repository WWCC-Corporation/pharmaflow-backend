using PharmaFlow.Application.Features.Clientes.DTOs;

namespace PharmaFlow.Application.Features.Clientes.Commands;

public class ActualizarClienteCommand
{
    public Guid Id { get; set; }

    public UpdateClienteDto Datos { get; set; } = null!;
}
