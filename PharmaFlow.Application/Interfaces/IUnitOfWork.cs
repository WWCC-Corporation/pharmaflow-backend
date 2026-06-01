namespace PharmaFlow.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    
    Task<int> SaveChangesAsync();
}
