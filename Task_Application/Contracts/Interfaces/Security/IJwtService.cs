using Task_Application.Dtos.Security;
using Task_Application.Dtos.User;
using Task_Domain.Entities;

namespace Task_Application.Contracts.Interfaces.Security
{
    public interface IJwtService
    {
        LoginResponseDto GenerateToken(User user);
    }
}
