using System.Text.Json.Serialization;

namespace Task_Application.Dtos.RefreshToken;

public sealed class RefreshAccessTokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string Role { get; set; } = string.Empty;
    public DateTime ExpireAt { get; set; }

    [JsonIgnore]
    public RefreshTokenCookieDto? RefreshTokenCookie { get; set; }
}
