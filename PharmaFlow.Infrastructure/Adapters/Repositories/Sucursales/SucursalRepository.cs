using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Sucursales;

public class SucursalRepository : ISucursalRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public SucursalRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Sucursale>> ListarAsync(bool? soloActivas, CancellationToken cancellationToken)
    {
        var sucursales = dbContext.Sucursales.AsNoTracking().AsQueryable();

        if (soloActivas.HasValue)
        {
            sucursales = sucursales.Where(sucursal => sucursal.Activo == soloActivas.Value);
        }

        return await sucursales
            .OrderBy(sucursal => sucursal.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Sucursale?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Sucursales.FirstOrDefaultAsync(sucursal => sucursal.Id == id, cancellationToken);
    }

    public async Task<bool> ExisteCodigoAsync(string codigo, Guid? idExcluir, CancellationToken cancellationToken)
    {
        var sucursales = dbContext.Sucursales.Where(sucursal => sucursal.Codigo == codigo);

        if (idExcluir.HasValue)
        {
            sucursales = sucursales.Where(sucursal => sucursal.Id != idExcluir.Value);
        }

        return await sucursales.AnyAsync(cancellationToken);
    }

    public async Task AgregarAsync(Sucursale sucursal, CancellationToken cancellationToken)
    {
        await dbContext.Sucursales.AddAsync(sucursal, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(Sucursale sucursal, CancellationToken cancellationToken)
    {
        dbContext.Sucursales.Update(sucursal);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
