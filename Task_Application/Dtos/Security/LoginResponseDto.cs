using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Task_Application.Dtos.RefreshToken;

namespace Task_Application.Dtos.Security
{
    public class LoginResponseDto
    {
        public string Token { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime ExpireAt { get; set; } = DateTime.Now;

        [JsonIgnore]
        public RefreshTokenCookieDto? RefreshTokenCookie { get; set; }
    }
}
