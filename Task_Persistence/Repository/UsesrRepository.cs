using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Contracts.Interfaces.Users;
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

        public async Task<bool> ExistsByUsernameAsync(string username)
        {
            return await _context.Set<User>().AnyAsync(x => x.Username == username);
        }

        public async Task<User?> GetUserByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(
                user => user.Username == username,
                cancellationToken);
        }

        public async Task<User?> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            string lowerUsernameOrEmail = usernameOrEmail.ToLower().Trim();

            return await _context.Set<User>()
                .FirstOrDefaultAsync(u => u.Username == lowerUsernameOrEmail || u.Email == lowerUsernameOrEmail);
        }
    }
}
