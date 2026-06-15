using Microsoft.EntityFrameworkCore;
using PharmaFlow.Application.Contracts.Persistence;
using PharmaFlow.Infrastructure.Context;
using PharmaFlow.Persistence;

namespace PharmaFlow.Infrastructure.Repositories.Compras;

public class ProveedorRepository : IProveedorRepository
{
    private readonly PharmaFlowDbContext _context;

    public ProveedorRepository(PharmaFlowDbContext context)
    {
        _context = context;
    }

    public async Task<List<Proveedore>> GetAllAsync()
    {
        return await _context.Proveedores
            .Where(p => p.Activo == true || p.Activo == null)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Proveedore?> GetByIdAsync(Guid id)
    {
        return await _context.Proveedores
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Proveedore> CreateAsync(Proveedore proveedor)
    {
        await _context.Proveedores.AddAsync(proveedor);
        await _context.SaveChangesAsync();

        return proveedor;
    }

    public async Task<Proveedore?> UpdateAsync(Proveedore proveedor)
    {
        var proveedorExistente = await _context.Proveedores
            .FirstOrDefaultAsync(p => p.Id == proveedor.Id);

        if (proveedorExistente is null)
        {
            return null;
        }

        proveedorExistente.Nombre = proveedor.Nombre;
        proveedorExistente.Ruc = proveedor.Ruc;
        proveedorExistente.Telefono = proveedor.Telefono;
        proveedorExistente.Correo = proveedor.Correo;
        proveedorExistente.Activo = proveedor.Activo;

        await _context.SaveChangesAsync();

        return proveedorExistente;
    }

    public async Task<bool> DeleteAsync(Guid id)
    {
        var proveedor = await _context.Proveedores
            .FirstOrDefaultAsync(p => p.Id == id);

        if (proveedor is null)
        {
            return false;
        }

        // Baja lógica: no se elimina físicamente, solo se desactiva.
        proveedor.Activo = false;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await _context.Proveedores
            .AnyAsync(p => p.Id == id && (p.Activo == true || p.Activo == null));
    }
}