using PharmaFlow.Application.Clientes.Commands;
using PharmaFlow.Application.Clientes.DTOs;
using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Clientes.Handlers;

public class CrearClienteHandler
{
    private readonly IClienteRepository clienteRepository;

    public CrearClienteHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<ClienteResponseDto> Handle(CrearClienteCommand command, CancellationToken cancellationToken)
    {
        var cliente = new Cliente
        {
            Dni = command.Datos.Dni,
            Nombres = command.Datos.Nombres,
            Apellidos = command.Datos.Apellidos,
            Telefono = command.Datos.Telefono,
            Correo = command.Datos.Correo
        };

        var resultado = await clienteRepository.CrearAsync(cliente, cancellationToken);

        return new ClienteResponseDto
        {
            Id = resultado.Id,
            Dni = resultado.Dni,
            Nombres = resultado.Nombres,
            Apellidos = resultado.Apellidos,
            Telefono = resultado.Telefono,
            Correo = resultado.Correo,
            Activo = resultado.Activo
        };
    }
}
