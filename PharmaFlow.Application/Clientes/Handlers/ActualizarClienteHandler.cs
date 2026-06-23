using PharmaFlow.Application.Features.Clientes.Commands;
using PharmaFlow.Application.Features.Clientes.DTOs;

namespace PharmaFlow.Application.Features.Clientes.Handlers;

public class ActualizarClienteHandler
{
    private readonly IClienteRepository clienteRepository;

    public ActualizarClienteHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public Task<ClienteResponseDto?> Handle(ActualizarClienteCommand command, CancellationToken cancellationToken)
    {
        return clienteRepository.ActualizarAsync(command.Id, command.Datos, cancellationToken);
    }
}
