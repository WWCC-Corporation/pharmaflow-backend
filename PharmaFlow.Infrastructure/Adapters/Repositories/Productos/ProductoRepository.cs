using Microsoft.EntityFrameworkCore;
using PharmaFlow.Domain.Entities;
using PharmaFlow.Domain.Ports.Repositories;
using PharmaFlow.Infrastructure.Data;

namespace PharmaFlow.Infrastructure.Adapters.Repositories.Productos;

public class ProductoRepository : IProductoRepository
{
    private readonly PharmaFlowDbContext dbContext;

    public ProductoRepository(PharmaFlowDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Producto>> ListarAsync(string? busqueda, bool? soloActivos, CancellationToken cancellationToken)
    {
        var productos = dbContext.Productos.AsNoTracking().AsQueryable();

        if (soloActivos.HasValue)
        {
            productos = productos.Where(producto => producto.Activo == soloActivos.Value);
        }

        if (!string.IsNullOrWhiteSpace(busqueda))
        {
            var texto = busqueda.Trim().ToLower();
            productos = productos.Where(producto =>
                producto.Nombre.ToLower().Contains(texto) ||
                (producto.CodigoBarra != null && producto.CodigoBarra.ToLower().Contains(texto)) ||
                (producto.PrincipioActivo != null && producto.PrincipioActivo.ToLower().Contains(texto)));
        }

        return await productos
            .OrderBy(producto => producto.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Producto?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await dbContext.Productos.FirstOrDefaultAsync(producto => producto.Id == id, cancellationToken);
    }

    public async Task<Producto?> ObtenerPorCodigoBarraAsync(string codigoBarra, CancellationToken cancellationToken)
    {
        return await dbContext.Productos
            .AsNoTracking()
            .FirstOrDefaultAsync(producto => producto.CodigoBarra == codigoBarra, cancellationToken);
    }

    public async Task<bool> ExisteCodigoBarraAsync(string codigoBarra, Guid? idExcluir, CancellationToken cancellationToken)
    {
        var productos = dbContext.Productos.Where(producto => producto.CodigoBarra == codigoBarra);

        if (idExcluir.HasValue)
        {
            productos = productos.Where(producto => producto.Id != idExcluir.Value);
        }

        return await productos.AnyAsync(cancellationToken);
    }

    public async Task AgregarAsync(Producto producto, CancellationToken cancellationToken)
    {
        await dbContext.Productos.AddAsync(producto, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task ActualizarAsync(Producto producto, CancellationToken cancellationToken)
    {
        dbContext.Productos.Update(producto);
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}
