using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.IRepositorios;

public interface IProductRepository
{
    Task<producto?> GetByIdAsync(Guid id);
    Task<IEnumerable<producto>> GetAllAsync();
    Task AddAsync(producto product);
    void Update(producto product);
    void Delete(producto product);
    Task SaveChangesAsync();
}