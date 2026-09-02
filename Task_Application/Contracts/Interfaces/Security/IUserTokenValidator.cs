namespace Task_Application.Contracts.Interfaces.Security;

public interface IUserTokenValidator
{
    Task<bool> IsValidAsync(
        Guid userId,
        int tokenVersion,
        CancellationToken cancellationToken = default);
}
