using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Infrastructure.Context;
using PharmaFlow.Persistence;

namespace PharmaFlow.Infrastructure.Repositories.Compras;

public class CompraRepository : ICompraRepository
{
    private readonly PharmaFlowDbContext _context;

    public CompraRepository(PharmaFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<Compra>> GetAllAsync()
    {
        return await _context.Compras
            .Include(c => c.IdProveedorNavigation)
            .Include(c => c.DetalleCompras)
                .ThenInclude(d => d.IdProductoNavigation)
            .OrderByDescending(c => c.Fecha)
            .ToListAsync();
    }

    public async Task<Compra?> GetByIdAsync(Guid id)
    {
        return await _context.Compras
            .Include(c => c.IdProveedorNavigation)
            .Include(c => c.DetalleCompras)
                .ThenInclude(d => d.IdProductoNavigation)
            .Include(c => c.Lotes)
            .Include(c => c.MovimientoInventarios)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Compra> CreateWithInventoryAsync(
        Compra compra,
        List<DetalleCompra> detalles,
        List<Lote> lotes,
        List<StockLote> stockLotes,
        List<MovimientoInventario> movimientos
    )
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            await _context.Compras.AddAsync(compra);
            await _context.DetalleCompras.AddRangeAsync(detalles);
            await _context.Lotes.AddRangeAsync(lotes);
            await _context.StockLotes.AddRangeAsync(stockLotes);
            await _context.MovimientoInventarios.AddRangeAsync(movimientos);

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return compra;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> ProveedorExistsAsync(Guid idProveedor)
    {
        return await _context.Proveedores
            .AnyAsync(p => p.Id == idProveedor && (p.Activo == true || p.Activo == null));
    }

    public async Task<bool> ProductoExistsAsync(Guid idProducto)
    {
        return await _context.Productos
            .AnyAsync(p => p.Id == idProducto);
    }
}