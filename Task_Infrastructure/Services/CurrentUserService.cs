using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Contracts.Interfaces.Users;
using Task_Domain.Enums;

namespace Task_Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public Guid? UserId
        {
            get
            {
                var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (Guid.TryParse(userIdClaim, out var parsedGuid))
                {
                    return parsedGuid;
                }

                return null;
            }
        }

        public UserRole? Role
        {
            get
            {
                string? roleClaim = _httpContextAccessor.HttpContext?
                    .User?
                    .FindFirst(ClaimTypes.Role)?
                    .Value;

                if (Enum.TryParse(
                        roleClaim,
                        ignoreCase: true,
                        out UserRole role))
                {
                    return role;
                }

                return null;
            }
        }

    }
}
