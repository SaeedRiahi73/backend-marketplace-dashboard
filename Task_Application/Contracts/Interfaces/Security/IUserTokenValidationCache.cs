using Task_Application.Models.Security;

namespace Task_Application.Contracts.Interfaces.Security;

public interface IUserTokenValidationCache
{
    bool TryGet(
        Guid userId,
        out UserTokenValidationState? state);

    void Set(
        Guid userId,
        UserTokenValidationState state);

    void Remove(Guid userId);
}
