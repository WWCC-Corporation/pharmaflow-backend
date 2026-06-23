using PharmaFlow.Application.Features.Clientes.Commands;
using PharmaFlow.Application.Features.Clientes.DTOs;

namespace PharmaFlow.Application.Features.Clientes.Handlers;

public class CrearClienteHandler
{
    private readonly IClienteRepository clienteRepository;

    public CrearClienteHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public Task<ClienteResponseDto> Handle(CrearClienteCommand command, CancellationToken cancellationToken)
    {
        return clienteRepository.CrearAsync(command.Datos, cancellationToken);
    }
}
