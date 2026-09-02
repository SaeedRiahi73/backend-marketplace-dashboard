namespace Task_Application.Contracts.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IReadOnlyList<T>> GetAllAsync(
            CancellationToken cancellationToken = default);
        Task<T?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
        Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default);
        void Delete(T entity);
    }
}
