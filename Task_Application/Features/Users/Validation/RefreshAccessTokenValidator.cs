using FluentValidation;
using Task_Application.Features.Users.Requests.Commands;

namespace Task_Application.Features.Users.Validation;

public sealed class RefreshAccessTokenValidator
    : AbstractValidator<RefreshAccessTokenCommandRequest>
{
    public RefreshAccessTokenValidator()
    {
        RuleFor(request => request.RefreshToken)
            .NotEmpty()
            .WithMessage("Refresh token is required.");
    }
}
