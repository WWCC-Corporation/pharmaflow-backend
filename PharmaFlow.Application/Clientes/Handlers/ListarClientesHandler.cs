using PharmaFlow.Application.Clientes.DTOs;
using PharmaFlow.Application.Clientes.Queries;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Clientes.Handlers;

public class ListarClientesHandler
{
    private readonly IClienteRepository clienteRepository;

    public ListarClientesHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<List<ClienteResponseDto>> Handle(ListarClientesQuery query, CancellationToken cancellationToken)
    {
        var clientes = await clienteRepository.ListarAsync(cancellationToken);

        return clientes.Select(c => new ClienteResponseDto
        {
            Id = c.Id,
            Dni = c.Dni,
            Nombres = c.Nombres,
            Apellidos = c.Apellidos,
            Telefono = c.Telefono,
            Correo = c.Correo,
            Activo = c.Activo
        }).ToList();
    }
}
