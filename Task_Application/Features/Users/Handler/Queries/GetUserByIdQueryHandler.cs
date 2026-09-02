using AutoMapper;
using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.User;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Queries;
using Task_Domain.Entities;
using Task_Domain.Enums;

namespace Task_Application.Features.Users.Handler.Queries;

public sealed class GetUserByIdQueryHandler
    : IRequestHandler<
        GetUserByIdQueryRequest,
        ResultInfo<GetUserByIdDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ResultInfo<GetUserByIdDto>> Handle(
        GetUserByIdQueryRequest request,
        CancellationToken cancellationToken)
    {
        Guid? currentUserId = _currentUserService.UserId;
        UserRole? currentUserRole = _currentUserService.Role;

        if (currentUserId is null || currentUserRole is null)
        {
            return ResultInfo<GetUserByIdDto>.Failure(
                ["The user is not authenticated."],
                status: ResultStatus.Unauthorized);
        }

        User? user = await _userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
        {
            return ResultInfo<GetUserByIdDto>.Failure(
                ["User not found."],
                status: ResultStatus.NotFound);
        }

        GetUserByIdDto response = _mapper.Map<GetUserByIdDto>(user);

        response.CanChangeStatus =
            currentUserRole == UserRole.Admin &&
            user.Role == UserRole.ProductManager &&
            !user.IsSystemUser &&
            user.Id != currentUserId.Value;

        return ResultInfo<GetUserByIdDto>.Success(
            response,
            "User retrieved successfully.");
    }
}
