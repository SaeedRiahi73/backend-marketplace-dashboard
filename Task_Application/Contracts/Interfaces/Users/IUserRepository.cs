using Task_Domain.Entities;


using Task_Application.Dtos.Common;
using Task_Application.Dtos.User;
using Task_Application.Models.User;

namespace Task_Application.Contracts.Interfaces.Users
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<bool> ExistsByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default);
        Task<bool> ExistsByEmailAsync(
            string email,
            CancellationToken cancellationToken = default);
        Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default);
        Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail);
        Task<PagedResultDto<UserListReadModel>> GetPagedUsersAsync(
            GetUsersFilterDto filter,
            CancellationToken cancellationToken = default);
    }
}
