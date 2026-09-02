namespace Task_Application.Dtos.User;

public sealed class GetUserByIdDto
{
    public Guid Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string Role { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public bool CanChangeStatus { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string? Image { get; set; }
}
