using PharmaFlow.Application.Sucursales.Commands;
using PharmaFlow.Application.Sucursales.DTOs;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;

namespace PharmaFlow.Application.Sucursales.Handlers;

public class CrearSucursalHandler
{
    private readonly ISucursalRepository sucursalRepository;

    public CrearSucursalHandler(ISucursalRepository sucursalRepository)
    {
        this.sucursalRepository = sucursalRepository;
    }

    public async Task<SucursalDto> Handle(CrearSucursalCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(command.Codigo))
        {
            throw new InvalidOperationException("El codigo de la sucursal es requerido.");
        }

        if (string.IsNullOrWhiteSpace(command.Nombre))
        {
            throw new InvalidOperationException("El nombre de la sucursal es requerido.");
        }

        var codigo = command.Codigo.Trim().ToUpperInvariant();
        if (await sucursalRepository.ExisteCodigoAsync(codigo, null, cancellationToken))
        {
            throw new InvalidOperationException("Ya existe una sucursal con ese codigo.");
        }

        var sucursal = new Sucursale
        {
            Codigo = codigo,
            Nombre = command.Nombre.Trim(),
            Direccion = NormalizarTexto(command.Direccion),
            Telefono = NormalizarTexto(command.Telefono),
            Activo = true
        };

        await sucursalRepository.AgregarAsync(sucursal, cancellationToken);

        return SucursalMapper.ToDto(sucursal);
    }

    private static string? NormalizarTexto(string? valor)
    {
        return string.IsNullOrWhiteSpace(valor) ? null : valor.Trim();
    }
}
