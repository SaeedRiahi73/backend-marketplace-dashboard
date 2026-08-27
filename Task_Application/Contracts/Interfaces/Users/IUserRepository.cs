using Task_Domain.Entities;


namespace Task_Application.Contracts.Interfaces.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> ExistsByUsernameAsync(string username);
        Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default);
        Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
    }
}
