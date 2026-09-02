using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Domain.Enums;

namespace Task_Application.Contracts.Interfaces.Users
{
    public interface ICurrentUserService
    {
        public Guid? UserId { get;}
        public UserRole? Role { get; }
    }
}
