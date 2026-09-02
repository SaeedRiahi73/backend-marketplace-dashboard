using Task_Application.Contracts.Interfaces;
using Task_Persistence.Context;

namespace Task_Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly TaskDbContext _context;

    public UnitOfWork(TaskDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(
        CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
