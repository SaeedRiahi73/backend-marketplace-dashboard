using AutoMapper;
using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.Services;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.User;
using Task_Application.Dtos.User.Profile;
using Task_Application.Enums;
using Task_Application.Features.Users.Requests.Commands;
using Task_Domain.Common;
using Task_Domain.Entities;
using Task_Domain.Enums;

namespace Task_Application.Features.Users.Handler.Commands;

public sealed class UpdateCurrentUserProfileCommandHandler
    : IRequestHandler<
        UpdateCurrentUserProfileCommandRequest,
        ResultInfo<CurrentUserDetailsDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IFileStorageService _fileStorageService;
    private readonly IMapper _mapper;

    public UpdateCurrentUserProfileCommandHandler(
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IFileStorageService fileStorageService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _fileStorageService = fileStorageService;
        _mapper = mapper;
    }

    public async Task<ResultInfo<CurrentUserDetailsDto>> Handle(UpdateCurrentUserProfileCommandRequest request, CancellationToken cancellationToken)
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

        if (user.Role == UserRole.Demo)
        {
            return ResultInfo<CurrentUserDetailsDto>.Failure(
                ["Demo users cannot update their profile."],
                status: ResultStatus.Forbidden);
        }

        UpdateCurrentUserProfileDto profile = request.Profile;

        bool usernameAlreadyExists =
            profile.Username is not null &&
            await _userRepository.ExistsByUsernameExceptUserAsync(
                profile.Username,
                user.Id,
                cancellationToken);

        if (usernameAlreadyExists)
            return ResultInfo<CurrentUserDetailsDto>.Failure(["Username already exists."], status: ResultStatus.Conflict);

        bool emailAlreadyExists =
            profile.Email is not null &&
            await _userRepository.ExistsByEmailExceptUserAsync(
                profile.Email,
                user.Id,
                cancellationToken);

        if (emailAlreadyExists)
            return ResultInfo<CurrentUserDetailsDto>.Failure(["Email already exists."], status: ResultStatus.Conflict);


        string? oldImagePath = user.Image;
        string? newImagePath = null;
        bool imageChanged = profile.ImageFile is not null || profile.RemoveImage;

        try
        {
            if (profile.ImageFile is not null)
            {
                newImagePath = await _fileStorageService.SaveFileAsync(
                    profile.ImageFile,
                    "user",
                    cancellationToken);

                user.ChangeImage(newImagePath);
            }
            else if (profile.RemoveImage)
            {
                user.ChangeImage(null);
            }

            if (profile.Username is not null)
                user.ChangeUsername(profile.Username);

            if (profile.Email is not null)
                user.ChangeEmail(profile.Email);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (DomainException exception)
        {
            DeleteNewImageIfNecessary(newImagePath);

            return ResultInfo<CurrentUserDetailsDto>.Failure([exception.Message], status: ResultStatus.BadRequest);
        }
        catch
        {
            DeleteNewImageIfNecessary(newImagePath);
            throw;
        }

        if (imageChanged && !string.IsNullOrWhiteSpace(oldImagePath))
            _fileStorageService.DeleteFile(oldImagePath);

        CurrentUserDetailsDto response =
            _mapper.Map<CurrentUserDetailsDto>(user);

        return ResultInfo<CurrentUserDetailsDto>.Success(response, "User profile updated successfully.");
    }

    private void DeleteNewImageIfNecessary(string? newImagePath)
    {
        if (!string.IsNullOrWhiteSpace(newImagePath))
            _fileStorageService.DeleteFile(newImagePath);
    }
}
