using PharmaFlow.Application.Features.Clientes.DTOs;
using PharmaFlow.Application.Features.Clientes.Queries;

namespace PharmaFlow.Application.Features.Clientes.Handlers;

public class ListarClientesHandler
{
    private readonly IClienteRepository clienteRepository;

    public ListarClientesHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public Task<List<ClienteResponseDto>> Handle(ListarClientesQuery query, CancellationToken cancellationToken)
    {
        return clienteRepository.ListarAsync(cancellationToken);
    }
}
