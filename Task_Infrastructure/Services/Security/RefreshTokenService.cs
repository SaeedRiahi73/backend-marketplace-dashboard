using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using Task_Application.Contracts.Interfaces.Security;

namespace Task_Infrastructure.Services.Security;

public sealed class RefreshTokenService : IRefreshTokenService
{
    private const int TokenSizeInBytes = 64;

    public string GenerateToken()
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(
            TokenSizeInBytes);

        return Base64UrlEncoder.Encode(randomBytes);
    }

    public string HashToken(string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            throw new ArgumentException(
                "Refresh token cannot be empty.",
                nameof(token));

        byte[] tokenBytes = Encoding.UTF8.GetBytes(token);
        byte[] hashBytes = SHA256.HashData(tokenBytes);

        return Convert.ToHexString(hashBytes);
    }
}
