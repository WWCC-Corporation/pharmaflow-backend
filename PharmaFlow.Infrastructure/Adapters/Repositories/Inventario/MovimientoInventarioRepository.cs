using System.Threading;
using System.Threading.Tasks;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Inventario;

public class MovimientoInventarioRepository : IMovimientoInventarioRepository
{
    private readonly PharmaFlowDbContext _dbContext;

    public MovimientoInventarioRepository(PharmaFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AgregarAsync(MovimientoInventario movimiento, CancellationToken cancellationToken = default)
    {
        await _dbContext.MovimientoInventarios.AddAsync(movimiento, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
}