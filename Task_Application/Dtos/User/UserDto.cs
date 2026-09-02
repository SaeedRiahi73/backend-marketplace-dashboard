using Task_Application.Dtos.Base;

namespace Task_Application.Dtos.User
{
    public class UserDto : BaseDto
    {
        public string Username { get; set; } = string.Empty;
        public int RoleId { get; set; }
        public string Role { get; set; } = string.Empty;
        public bool IsActive { get; set; }
        public bool CanChangeStatus { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Image { get; set; }
    }
}
