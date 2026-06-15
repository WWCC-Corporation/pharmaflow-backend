using PharmaFlow.Application.Features.Clientes.Commands;

namespace PharmaFlow.Application.Features.Clientes.Handlers;

public class DesactivarClienteHandler
{
    private readonly IClienteRepository clienteRepository;

    public DesactivarClienteHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public Task<bool> Handle(DesactivarClienteCommand command, CancellationToken cancellationToken)
    {
        return clienteRepository.DesactivarAsync(command.Id, cancellationToken);
    }
}
