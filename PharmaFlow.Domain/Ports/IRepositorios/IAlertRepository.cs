using PharmaFlow.Domain.Entities;

namespace PharmaFlow.Domain.Ports.IRepositorios;

public interface IAlertRepository
{
    Task<alerta?> GetByIdAsync(Guid id);
    Task<IEnumerable<alerta>> GetUnreadAlertsAsync();
    Task AddAsync(alerta alert);
    void Update(alerta alert);
    Task SaveChangesAsync();
}