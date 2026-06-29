namespace PharmaFlow.Application.Clientes.Handlers;

using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;

public interface IClienteRepository
{
    Task<List<Cliente>> ListarAsync(CancellationToken cancellationToken);

    Task<Cliente?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken);

    Task<Cliente> CrearAsync(Cliente cliente, CancellationToken cancellationToken);

    Task<Cliente?> ActualizarAsync(Cliente cliente, CancellationToken cancellationToken);

    Task<bool> DesactivarAsync(Guid id, CancellationToken cancellationToken);
}
