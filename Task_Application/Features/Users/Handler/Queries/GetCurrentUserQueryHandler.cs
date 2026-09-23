using AutoMapper;
using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.User;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Queries;
using Task_Domain.Entities;

namespace Task_Application.Features.Users.Handler.Queries;

public sealed class GetCurrentUserQueryHandler
    : IRequestHandler<
        GetCurrentUserQueryRequest,
        ResultInfo<CurrentUserDetailsDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly ICurrentUserService _currentUserService;
    private readonly IMapper _mapper;

    public GetCurrentUserQueryHandler(
        IUserRepository userRepository,
        ICurrentUserService currentUserService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _currentUserService = currentUserService;
        _mapper = mapper;
    }

    public async Task<ResultInfo<CurrentUserDetailsDto>> Handle(
        GetCurrentUserQueryRequest request,
        CancellationToken cancellationToken)
    {
        Guid? currentUserId = _currentUserService.UserId;

        if (currentUserId is null)
        {
            return ResultInfo<CurrentUserDetailsDto>.Failure(
                ["The user is not authenticated."],
                status: ResultStatus.Unauthorized);
        }

        User? user = await _userRepository.GetByIdAsync(
            currentUserId.Value,
            cancellationToken);

        if (user is null)
        {
            return ResultInfo<CurrentUserDetailsDto>.Failure(
                ["User not found."],
                status: ResultStatus.NotFound);
        }

        CurrentUserDetailsDto response =
            _mapper.Map<CurrentUserDetailsDto>(user);

        return ResultInfo<CurrentUserDetailsDto>.Success(
            response,
            "Current user retrieved successfully.");
    }
}
