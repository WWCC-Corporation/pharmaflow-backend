using PharmaFlow.Application.Interfaces;
using PharmaFlow.Infrastructure.Context;

namespace PharmaFlow.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly PharmaFlowDbContext _context;

    public UnitOfWork(PharmaFlowDbContext context)
    {
        _context = context;
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}
