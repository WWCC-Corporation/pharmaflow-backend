using PharmaFlow.Application.Clientes.Commands;
using PharmaFlow.Application.Clientes.DTOs;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Application.Clientes.Handlers;

public class ActualizarClienteHandler
{
    private readonly IClienteRepository clienteRepository;

    public ActualizarClienteHandler(IClienteRepository clienteRepository)
    {
        this.clienteRepository = clienteRepository;
    }

    public async Task<ClienteResponseDto?> Handle(ActualizarClienteCommand command, CancellationToken cancellationToken)
    {
        var cliente = new Cliente
        {
            Id = command.Id,
            Dni = command.Datos.Dni,
            Nombres = command.Datos.Nombres,
            Apellidos = command.Datos.Apellidos,
            Telefono = command.Datos.Telefono,
            Correo = command.Datos.Correo,
            Activo = command.Datos.Activo
        };

        var resultado = await clienteRepository.ActualizarAsync(cliente, cancellationToken);

        if (resultado is null)
        {
            return null;
        }

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
