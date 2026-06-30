using PharmaFlow.Application.Sucursales.Commands;
using PharmaFlow.Application.Sucursales.DTOs;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Sucursales.Handlers;

public class ActualizarSucursalHandler
{
    private readonly ISucursalRepository sucursalRepository;

    public ActualizarSucursalHandler(ISucursalRepository sucursalRepository)
    {
        this.sucursalRepository = sucursalRepository;
    }

    public async Task<SucursalDto?> Handle(ActualizarSucursalCommand command, CancellationToken cancellationToken)
    {
        var sucursal = await sucursalRepository.ObtenerPorIdAsync(command.Id, cancellationToken);

        if (sucursal is null)
        {
            return null;
        }

        if (command.Codigo is not null)
        {
            if (string.IsNullOrWhiteSpace(command.Codigo))
            {
                throw new InvalidOperationException("El codigo de la sucursal no puede estar vacio.");
            }

            var codigo = command.Codigo.Trim().ToUpperInvariant();
            if (await sucursalRepository.ExisteCodigoAsync(codigo, sucursal.Id, cancellationToken))
            {
                throw new InvalidOperationException("Ya existe una sucursal con ese codigo.");
            }

            sucursal.Codigo = codigo;
        }

        if (command.Nombre is not null)
        {
            if (string.IsNullOrWhiteSpace(command.Nombre))
            {
                throw new InvalidOperationException("El nombre de la sucursal no puede estar vacio.");
            }

            sucursal.Nombre = command.Nombre.Trim();
        }

        if (command.Direccion is not null) sucursal.Direccion = NormalizarTexto(command.Direccion);
        if (command.Telefono is not null) sucursal.Telefono = NormalizarTexto(command.Telefono);
        if (command.Activo.HasValue) sucursal.Activo = command.Activo.Value;
        sucursal.UpdatedAt = DateTime.UtcNow;

        await sucursalRepository.ActualizarAsync(sucursal, cancellationToken);

        return SucursalMapper.ToDto(sucursal);
    }

    private static string? NormalizarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
