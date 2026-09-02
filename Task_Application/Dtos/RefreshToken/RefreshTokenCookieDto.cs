namespace Task_Application.Dtos.RefreshToken;

public sealed class RefreshTokenCookieDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public bool IsPersistent { get; set; }
}
