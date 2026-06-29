using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Application.Clientes.Handlers;
using PharmaFlow.Infrastructure.Data;

    
namespace PharmaFlow.Infrastructure.Repositories.Clientes;

public class ClienteRepository : IClienteRepository
{
    private readonly PharmaFlowDbContext context;

    public ClienteRepository(PharmaFlowDbContext context)
    {
        this.context = context;
    }

    public async Task<List<Cliente>> ListarAsync(CancellationToken cancellationToken)
    {
        return await context.Clientes
            .AsNoTracking()
            .OrderBy(c => c.Apellidos)
            .ThenBy(c => c.Nombres)
            .ToListAsync(cancellationToken);
    }

    public async Task<Cliente?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Clientes
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
    }

    public async Task<Cliente> CrearAsync(Cliente cliente, CancellationToken cancellationToken)
    {
        ValidarDatosCreacion(cliente);
        await ValidarDniUnicoAsync(cliente.Dni, null, cancellationToken);

        cliente.Id = Guid.NewGuid();
        cliente.Activo = true;

        context.Clientes.Add(cliente);
        await context.SaveChangesAsync(cancellationToken);

        return cliente;
    }

    public async Task<Cliente?> ActualizarAsync(Cliente cliente, CancellationToken cancellationToken)
    {
        var existing = await context.Clientes.FirstOrDefaultAsync(c => c.Id == cliente.Id, cancellationToken);

        if (existing is null)
        {
            return null;
        }

        if (cliente.Dni is not null)
        {
            var dni = cliente.Dni.Trim();

            if (string.IsNullOrWhiteSpace(dni))
            {
                throw new ArgumentException("El DNI no puede estar vacío.");
            }

            await ValidarDniUnicoAsync(dni, cliente.Id, cancellationToken);
            existing.Dni = dni;
        }

        if (cliente.Nombres is not null)
        {
            if (string.IsNullOrWhiteSpace(cliente.Nombres))
            {
                throw new ArgumentException("Los nombres no pueden estar vacíos.");
            }

            existing.Nombres = cliente.Nombres.Trim();
        }

        if (cliente.Apellidos is not null)
        {
            existing.Apellidos = cliente.Apellidos.Trim();
        }

        if (cliente.Telefono is not null)
        {
            existing.Telefono = cliente.Telefono.Trim();
        }

        if (cliente.Correo is not null)
        {
            existing.Correo = cliente.Correo.Trim();
        }

        //if (cliente.Activo.HasValue)
        //{
        //    existing.Activo = cliente.Activo.Value;
        //}

        await context.SaveChangesAsync(cancellationToken);

        return existing;
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

    private static void ValidarDatosCreacion(Cliente cliente)
    {
        if (string.IsNullOrWhiteSpace(cliente.Nombres))
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
}
