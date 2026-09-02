using Task_Domain.Enums;

namespace Task_Application.Models.User;

public sealed class UserListReadModel
{
    public Guid Id { get; init; }
    public string Username { get; init; } = string.Empty;
    public UserRole Role { get; init; }
    public bool IsActive { get; init; }
    public bool IsSystemUser { get; init; }
    public DateTime CreatedAt { get; init; }
    public string? Image { get; init; }
}
