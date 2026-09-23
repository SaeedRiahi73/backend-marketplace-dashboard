using Microsoft.AspNetCore.Http;

namespace Task_Application.Dtos.User.Profile;

public sealed class UpdateCurrentUserProfileDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public IFormFile? ImageFile { get; set; }
    public bool RemoveImage { get; set; }
}
