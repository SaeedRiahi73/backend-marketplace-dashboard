using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task_Application.Dtos.User
{
    public class UserLoginDto
    {
        public string UsernameOrEmail { get; init; }
        public string Password { get; init; }
        public bool RememberMe { get; init; } = false;
    }
}
