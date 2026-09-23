using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.Common;
using Task_Application.Dtos.User;
using Task_Application.Enums;
using Task_Application.Models.User;
using Task_Domain.Entities;
using Task_Persistence.Context;

namespace Task_Persistence.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly TaskDbContext _context;

        public UserRepository(TaskDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistsByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            string normalizedUsername = username
                .Trim()
                .ToLowerInvariant();

            return await _context.Users.AnyAsync(user => user.NormalizedUsername == normalizedUsername, cancellationToken);
        }

        public async Task<bool> ExistsByUsernameExceptUserAsync(string username, Guid excludedUserId, CancellationToken cancellationToken = default)
        {
            string normalizedUsername = username
                .Trim()
                .ToLowerInvariant();

            return await _context.Users.AnyAsync(
                user => user.Id != excludedUserId &&
                        user.NormalizedUsername == normalizedUsername,
                cancellationToken);
        }

        public async Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            string normalizedEmail = email
                .Trim()
                .ToLowerInvariant();

            return await _context.Users.AnyAsync(user => user.Email == normalizedEmail, cancellationToken);
        }

        public async Task<bool> ExistsByEmailExceptUserAsync(string email, Guid excludedUserId, CancellationToken cancellationToken = default)
        {
            string normalizedEmail = email
                .Trim()
                .ToLowerInvariant();

            return await _context.Users.AnyAsync(
                user => user.Id != excludedUserId &&
                        user.Email == normalizedEmail,
                cancellationToken);
        }

        public async Task<User?> GetUserByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            string normalizedUsername = username
                .Trim()
                .ToLowerInvariant();

            return await _context.Set<User>().FirstOrDefaultAsync(
                user => user.NormalizedUsername == normalizedUsername,
                cancellationToken);
        }

        public async Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            string normalizedUsernameOrEmail = usernameOrEmail
                .Trim()
                .ToLowerInvariant();

            return await _context.Set<User>()
                .FirstOrDefaultAsync(user =>
                    user.NormalizedUsername == normalizedUsernameOrEmail ||
                    user.Email == normalizedUsernameOrEmail);
        }

        public async Task<UserDetailsReadModel?> GetUserDetailsByIdAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            return await _context.Users
                .AsNoTracking()
                .Where(user => user.Id == userId)
                .Select(user => new UserDetailsReadModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    IsSystemUser = user.IsSystemUser,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt,
                    Image = user.Image
                })
                .SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<PagedResultDto<UserListReadModel>> GetPagedUsersAsync(GetUsersFilterDto filter, CancellationToken cancellationToken = default)
        {
            IQueryable<User> query = _context.Users.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                string normalizedSearch = filter.Search
                    .Trim()
                    .ToLowerInvariant();

                query = query.Where(user =>
                    user.NormalizedUsername.Contains(normalizedSearch));
            }

            if (filter.Role.HasValue)
            {
                query = query.Where(user =>
                    user.Role == filter.Role.Value);
            }

            if (filter.IsActive.HasValue)
            {
                query = query.Where(user =>
                    user.IsActive == filter.IsActive.Value);
            }

            int totalCount = await query.CountAsync(cancellationToken);

            query = filter.SortOrder == UserSortOrder.Oldest
                ? query.OrderBy(user => user.CreatedAt)
                    .ThenBy(user => user.Id)
                : query.OrderByDescending(user => user.CreatedAt)
                    .ThenByDescending(user => user.Id);

            IReadOnlyList<UserListReadModel> items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(user => new UserListReadModel
                {
                    Id = user.Id,
                    Username = user.Username,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    IsSystemUser = user.IsSystemUser,
                    CreatedAt = user.CreatedAt,
                    Image = user.Image
                })
                .ToListAsync(cancellationToken);

            return new PagedResultDto<UserListReadModel>
            {
                Items = items,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(
                    totalCount / (double)filter.PageSize)
            };
        }
    }
}
