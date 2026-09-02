namespace Task_Application.Contracts.Interfaces.Security;

public interface IRefreshTokenService
{
    string GenerateToken();
    string HashToken(string token);
}
