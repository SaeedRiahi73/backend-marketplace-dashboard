using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Task_Application.Common.Validation;

public sealed class ImageFileValidator : AbstractValidator<IFormFile?>
{
    private const long MaxImageSizeInBytes = 2 * 1024 * 1024;

    public ImageFileValidator()
    {
        RuleFor(image => image)
            .Must(image => image is null || image.Length > 0)
            .WithMessage("The selected image is empty.")
            .Must(image => image is null || image.Length <= MaxImageSizeInBytes)
            .WithMessage("The image size cannot exceed 2 MB.")
            .Must(HaveAllowedImageFormat)
            .WithMessage("Only JPG, JPEG, PNG, and WEBP images are allowed.");
    }

    private static bool HaveAllowedImageFormat(IFormFile? image)
    {
        if (image is null)
            return true;

        string extension = Path
            .GetExtension(image.FileName)
            .ToLowerInvariant();

        return ((extension is ".jpg" or ".jpeg") &&
                image.ContentType.Equals("image/jpeg", StringComparison.OrdinalIgnoreCase)) ||
               (extension == ".png" &&
                image.ContentType.Equals("image/png", StringComparison.OrdinalIgnoreCase)) ||
               (extension == ".webp" &&
                image.ContentType.Equals("image/webp", StringComparison.OrdinalIgnoreCase));
    }
}
