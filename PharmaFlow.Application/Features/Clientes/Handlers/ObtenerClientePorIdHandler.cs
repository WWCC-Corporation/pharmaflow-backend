using PharmaFlow.Application.Features.Clientes.DTOs;
using PharmaFlow.Application.Features.Clientes.Queries;

namespace PharmaFlow.Application.Features.Clientes.Handlers;

public class ObtenerClientePorIdHandler
{
    private readonly IClienteRepository clienteRepository;

    public ObtenerClientePorIdHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public Task<ClienteResponseDto?> Handle(ObtenerClientePorIdQuery query, CancellationToken cancellationToken)
    {
        return clienteRepository.ObtenerPorIdAsync(query.Id, cancellationToken);
    }
}
