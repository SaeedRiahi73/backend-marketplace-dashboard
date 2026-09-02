using AutoMapper;
using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.Common;
using Task_Application.Dtos.User;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Queries;
using Task_Application.Models.User;
using Task_Domain.Enums;

namespace Task_Application.Features.Users.Handler.Queries;

public sealed class GetUsersListQueryHandler
    : IRequestHandler<
        GetUsersListQueryRequest,
        ResultInfo<PagedResultDto<UserDto>>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUsersListQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ResultInfo<PagedResultDto<UserDto>>> Handle(GetUsersListQueryRequest request,CancellationToken cancellationToken)
    {
        Guid? currentUserId = _currentUserService.UserId;
        UserRole? currentRole = _currentUserService.Role;

        if (currentUserId is null || currentRole is null)
            return ResultInfo<PagedResultDto<UserDto>>.Failure(["The user is not authenticated."],status: ResultStatus.Unauthorized);

        if (currentRole != UserRole.Admin && currentRole != UserRole.Demo)
            return ResultInfo<PagedResultDto<UserDto>>.Failure(["The user does not have access to the user list."],status: ResultStatus.Forbidden);

        PagedResultDto<UserListReadModel> usersPage =
            await _userRepository.GetPagedUsersAsync(
                request.Filter,
                cancellationToken);

        List<UserDto> users = usersPage.Items
            .Select(user =>
            {
                UserDto dto = _mapper.Map<UserDto>(user);

                dto.CanChangeStatus =
                    currentRole == UserRole.Admin &&
                    user.Role == UserRole.ProductManager &&
                    !user.IsSystemUser &&
                    user.Id != currentUserId.Value;

                return dto;
            })
            .ToList();

        PagedResultDto<UserDto> response = new()
        {
            Items = users,
            PageNumber = usersPage.PageNumber,
            PageSize = usersPage.PageSize,
            TotalCount = usersPage.TotalCount,
            TotalPages = usersPage.TotalPages
        };

        return ResultInfo<PagedResultDto<UserDto>>.Success(response,"Users retrieved successfully.");
    }
}
