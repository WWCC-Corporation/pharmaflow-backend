using System;
using System.Threading;
using System.Threading.Tasks;
using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.Repositories;

public interface IMovimientoInventarioRepository
{
    Task AgregarAsync(MovimientoInventario movimiento, CancellationToken cancellationToken = default);
}