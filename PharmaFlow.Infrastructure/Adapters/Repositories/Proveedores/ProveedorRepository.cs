using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Proveedores;

public class ProveedorRepository : IProveedorRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public ProveedorRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<Proveedore?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Proveedores
            .FirstOrDefaultAsync(proveedor => proveedor.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Proveedore>> ListarAsync(bool? soloActivos, CancellationToken cancellationToken)
    {
        var proveedores = dbContext.Proveedores.AsNoTracking();

        if (soloActivos.HasValue)
        {
            proveedores = proveedores.Where(proveedor => proveedor.Activo == soloActivos.Value);
        }

        return await proveedores
            .OrderBy(proveedor => proveedor.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExisteRucAsync(string ruc, Guid? idExcluir, CancellationToken cancellationToken)
    {
        var proveedores = dbContext.Proveedores.Where(proveedor => proveedor.Ruc == ruc);

        if (idExcluir.HasValue)
        {
            proveedores = proveedores.Where(proveedor => proveedor.Id != idExcluir.Value);
        }

        return await proveedores.AnyAsync(cancellationToken);
    }

    public async Task AgregarAsync(Proveedore proveedor, CancellationToken cancellationToken)
    {
        await dbContext.Proveedores.AddAsync(proveedor, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(Proveedore proveedor, CancellationToken cancellationToken)
    {
        dbContext.Proveedores.Update(proveedor);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
