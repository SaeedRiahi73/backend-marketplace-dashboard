using Task_Application.Dtos.Base;

namespace Task_Application.Dtos.User
{
    public class UserDto : BaseDto
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
