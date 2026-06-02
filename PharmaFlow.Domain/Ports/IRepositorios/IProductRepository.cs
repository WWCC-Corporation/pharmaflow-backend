using PharmaFlow.Persistence;

namespace PharmaFlow.Domain.Ports.IRepositorios;

public interface IProductRepository
{
    Task<Producto?> GetByIdAsync(Guid id);
    Task<IEnumerable<Producto>> GetAllAsync();
    Task AddAsync(Producto product);
    void Update(Producto product);
    void Delete(Producto product);
    Task SaveChangesAsync();
}