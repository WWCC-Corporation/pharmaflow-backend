using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Features.Clientes.DTOs;
using PharmaFlow.Application.Features.Clientes.Handlers;
using PharmaFlow.Infrastructure.Context;
using PharmaFlow.Persistence;

namespace PharmaFlow.Infrastructure.Repositories.Clientes;

public class ClienteRepository : IClienteRepository
{
    private readonly PharmaFlowDbContext context;

    public ClienteRepository(PharmaFlowDbContext context)
    {
        this.context = context;
    }

    public async Task<List<ClienteResponseDto>> ListarAsync(CancellationToken cancellationToken)
    {
        var clientes = await context.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Apellidos)
            .ThenBy(c => c.Nombres)
            .ToListAsync(cancellationToken);

        return clientes.Select(MapToDto).ToList();
    }

    public async Task<ClienteResponseDto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var cliente = await context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        return cliente is null ? null : MapToDto(cliente);
    }

    public async Task<ClienteResponseDto> CrearAsync(CreateClienteDto dto, CancellationToken cancellationToken)
    {
        ValidarDatosCreacion(dto);
        await ValidarDniUnicoAsync(dto.Dni, null, cancellationToken);

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Dni = dto.Dni?.Trim(),
            Nombres = dto.Nombres?.Trim(),
            Apellidos = dto.Apellidos?.Trim(),
            Telefono = dto.Telefono?.Trim(),
            Correo = dto.Correo?.Trim(),
            Activo = true
        };

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(cliente);
    }

    public async Task<ClienteResponseDto?> ActualizarAsync(
        Guid id,
        UpdateClienteDto dto,
        CancellationToken cancellationToken)
    {
        var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (cliente is null)
        {
            return null;
        }

        if (dto.Dni is not null)
        {
            var dni = dto.Dni.Trim();

            if (string.IsNullOrWhiteSpace(dni))
            {
                throw new ArgumentException("El DNI no puede estar vacío.");
            }

            await ValidarDniUnicoAsync(dni, id, cancellationToken);
            cliente.Dni = dni;
        }

        if (dto.Nombres is not null)
        {
            if (string.IsNullOrWhiteSpace(dto.Nombres))
            {
                throw new ArgumentException("Los nombres no pueden estar vacíos.");
            }

            cliente.Nombres = dto.Nombres.Trim();
        }

        if (dto.Apellidos is not null)
        {
            cliente.Apellidos = dto.Apellidos.Trim();
        }

        if (dto.Telefono is not null)
        {
            cliente.Telefono = dto.Telefono.Trim();
        }

        if (dto.Correo is not null)
        {
            cliente.Correo = dto.Correo.Trim();
        }

        if (dto.Activo.HasValue)
        {
            cliente.Activo = dto.Activo.Value;
        }

        await context.SaveChangesAsync(cancellationToken);

        return MapToDto(cliente);
    }

    public async Task<bool> DesactivarAsync(Guid id, CancellationToken cancellationToken)
    {
        var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

        if (cliente is null)
        {
            return false;
        }

        cliente.Activo = false;
        await context.SaveChangesAsync(cancellationToken);

        return true;
    }

    private static void ValidarDatosCreacion(CreateClienteDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Nombres))
        {
            throw new ArgumentException("Los nombres son requeridos.");
        }
    }

    private async Task ValidarDniUnicoAsync(string? dni, Guid? idExcluir, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(dni))
        {
            return;
        }

        var dniNormalizado = dni.Trim();

        var existe = await context.Clientes.AnyAsync(
            c => c.Dni == dniNormalizado && (!idExcluir.HasValue || c.Id != idExcluir.Value),
            cancellationToken);

        if (existe)
        {
            throw new ArgumentException("Ya existe un cliente con ese DNI.");
        }
    }

    private static ClienteResponseDto MapToDto(Cliente cliente)
    {
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
