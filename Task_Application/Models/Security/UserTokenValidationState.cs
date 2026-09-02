namespace Task_Application.Models.Security;

public sealed record UserTokenValidationState(
    bool IsActive,
    int TokenVersion);
