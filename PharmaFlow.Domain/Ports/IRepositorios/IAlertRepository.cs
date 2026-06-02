using PharmaFlow.Persistence;

namespace PharmaFlow.Domain.Ports.IRepositorios;

public interface IAlertRepository
{
    Task<Alerta?> GetByIdAsync(Guid id);
    Task<IEnumerable<Alerta>> GetUnreadAlertsAsync();
    Task AddAsync(Alerta alert);
    void Update(Alerta alert);
    Task SaveChangesAsync();
}