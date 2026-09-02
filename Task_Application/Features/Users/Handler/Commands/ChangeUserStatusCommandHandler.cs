using AutoMapper;
using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.Security;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.User;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Common;
using Task_Domain.Entities;
using Task_Domain.Enums;

namespace Task_Application.Features.Users.Handler.Command;

public sealed class ChangeUserStatusCommandHandler
    : IRequestHandler<ChangeUserStatusCommandRequest, ResultInfo<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IUserTokenValidationCache _tokenValidationCache;
    private readonly IMapper _mapper;

    public ChangeUserStatusCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IUserTokenValidationCache tokenValidationCache,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _tokenValidationCache = tokenValidationCache;
        _mapper = mapper;
    }

    public async Task<ResultInfo<UserDto>> Handle(
        ChangeUserStatusCommandRequest request,
        CancellationToken cancellationToken)
    {
        Guid? currentUserId = _currentUserService.UserId;
        UserRole? currentUserRole = _currentUserService.Role;

        if (currentUserId is null || currentUserRole is null)
        {
            return ResultInfo<UserDto>.Failure(
                ["The user is not authenticated."],
                status: ResultStatus.Unauthorized);
        }

        if (currentUserRole != UserRole.Admin)
        {
            return ResultInfo<UserDto>.Failure(
                ["Only administrators can change user status."],
                status: ResultStatus.Forbidden);
        }

        if (currentUserId.Value == request.UserId)
        {
            return ResultInfo<UserDto>.Failure(
                ["You cannot change your own status."],
                status: ResultStatus.BadRequest);
        }

        User? user = await _userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);

        if (user is null)
        {
            return ResultInfo<UserDto>.Failure(
                ["User not found."],
                status: ResultStatus.NotFound);
        }

        try
        {
            user.ChangeStatus(request.UserStatus.IsActive);
        }
        catch (DomainException exception)
        {
            return ResultInfo<UserDto>.Failure(
                [exception.Message],
                status: ResultStatus.BadRequest);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        _tokenValidationCache.Remove(user.Id);

        UserDto response = _mapper.Map<UserDto>(user);
        response.CanChangeStatus = true;

        return ResultInfo<UserDto>.Success(
            response,
            "User status changed successfully.");
    }
}
