using PharmaFlow.Application.Clientes.DTOs;
using PharmaFlow.Application.Clientes.Queries;

namespace PharmaFlow.Application.Clientes.Handlers;

public class ObtenerClientePorIdHandler
{
    private readonly IClienteRepository clienteRepository;

    public ObtenerClientePorIdHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<ClienteResponseDto?> Handle(ObtenerClientePorIdQuery query, CancellationToken cancellationToken)
    {
        var cliente = await clienteRepository.ObtenerPorIdAsync(query.Id, cancellationToken);

        if (cliente is null)
        {
            return null;
        }

        return new ClienteResponseDto
        {
            Id = cliente.Id,
            Dni = cliente.Dni,
            Nombres = cliente.Nombres,
            Apellidos = cliente.Apellidos,
            Telefono = cliente.Telefono,
            Correo = cliente.Correo,
            Activo = cliente.Activo
        };
    }
}
