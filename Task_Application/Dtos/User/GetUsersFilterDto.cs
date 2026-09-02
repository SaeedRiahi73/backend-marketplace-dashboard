using Task_Application.Enums;
using Task_Domain.Enums;

namespace Task_Application.Dtos.User;

public sealed class GetUsersFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public UserRole? Role { get; set; }
    public bool? IsActive { get; set; }
    public UserSortOrder SortOrder { get; set; } = UserSortOrder.Newest;
}
