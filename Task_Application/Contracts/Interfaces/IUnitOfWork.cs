namespace Task_Application.Contracts.Interfaces;

public interface IUnitOfWork
{
    Task SaveChangesAsync(
        CancellationToken cancellationToken = default);
}
