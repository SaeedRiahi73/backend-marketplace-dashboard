using FluentValidation;
using Microsoft.AspNetCore.Http;
using Task_Application.Features.Users.Requests.Commands;

namespace Task_Application.Features.Users.Validation;

public sealed class UpdateCurrentUserProfileValidator
    : AbstractValidator<UpdateCurrentUserProfileCommandRequest>
{
    private const long MaxImageSizeInBytes = 2 * 1024 * 1024;

    private static readonly HashSet<string> AllowedImageExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png",
            ".webp"
        };

    private static readonly HashSet<string> AllowedImageContentTypes =
        new(StringComparer.OrdinalIgnoreCase)
        {
            "image/jpeg",
            "image/png",
            "image/webp"
        };

    public UpdateCurrentUserProfileValidator()
    {
        RuleFor(command => command.Profile)
            .NotNull()
            .WithMessage("Profile data is required.")
            .Must(profile => profile is not null &&
                (!string.IsNullOrWhiteSpace(profile.Username) ||
                 !string.IsNullOrWhiteSpace(profile.Email) ||
                 profile.ImageFile is not null ||
                 profile.RemoveImage))
            .WithMessage("At least one profile field must be changed.");

        When(command => command.Profile is not null, () =>
        {
            RuleFor(command => command.Profile.Username)
                .NotEmpty()
                .WithMessage("Username cannot be empty.")
                .Must(username => username is not null &&
                    username.Trim().Length >= 3)
                .WithMessage("Username must be at least 3 characters.")
                .Must(username => username is not null &&
                    username.Trim().Length <= 50)
                .WithMessage("Username cannot exceed 50 characters.")
                .Matches("^[a-zA-Z0-9._ -]+$")
                .WithMessage(
                    "Username can only contain letters, numbers, spaces, dots, underscores, and hyphens.")
                .When(command => command.Profile.Username is not null);

            RuleFor(command => command.Profile.Email)
                .NotEmpty()
                .WithMessage("Email cannot be empty.")
                .EmailAddress()
                .WithMessage("Email format is invalid.")
                .MaximumLength(50)
                .WithMessage("Email cannot exceed 50 characters.")
                .When(command => command.Profile.Email is not null);

            RuleFor(command => command.Profile)
                .Must(profile => profile.ImageFile is null || !profile.RemoveImage)
                .WithMessage(
                    "A new image cannot be uploaded while removing the current image.");

            RuleFor(command => command.Profile.ImageFile)
                .Must(image => image is null || image.Length > 0)
                .WithMessage("The selected image is empty.")
                .Must(image => image is null || image.Length <= MaxImageSizeInBytes)
                .WithMessage("The image size cannot exceed 2 MB.")
                .Must(HaveAllowedImageFormat)
                .WithMessage("Only JPG, JPEG, PNG, and WEBP images are allowed.");
        });
    }

    private static bool HaveAllowedImageFormat(IFormFile? image)
    {
        if (image is null)
            return true;

        string extension = Path.GetExtension(image.FileName);

        return AllowedImageExtensions.Contains(extension) &&
               AllowedImageContentTypes.Contains(image.ContentType);
    }
}
