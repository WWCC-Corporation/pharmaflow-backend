using PharmaFlow.Persistence;

namespace PharmaFlow.Application.Features.Compras.Interfaces;

public interface IProveedorRepository
{
    Task<List<Proveedore>> GetAllAsync();
    Task<Proveedore?> GetByIdAsync(Guid id);
    Task<Proveedore> CreateAsync(Proveedore proveedor);
    Task<Proveedore?> UpdateAsync(Proveedore proveedor);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ExistsAsync(Guid id);
}