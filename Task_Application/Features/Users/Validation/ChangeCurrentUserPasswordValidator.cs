using FluentValidation;
using Task_Application.Features.Users.Requests.Commands;

namespace Task_Application.Features.Users.Validation;

public sealed class ChangeCurrentUserPasswordValidator
    : AbstractValidator<ChangeCurrentUserPasswordCommandRequest>
{
    public ChangeCurrentUserPasswordValidator()
    {
        RuleFor(command => command.Password)
            .NotNull()
            .WithMessage("Password data is required.");

        When(command => command.Password is not null, () =>
        {
            RuleFor(command => command.Password.CurrentPassword)
                .NotEmpty()
                .WithMessage("Current password is required.");

            RuleFor(command => command.Password.NewPassword)
                .NotEmpty()
                .WithMessage("New password is required.")
                .MinimumLength(8)
                .WithMessage("New password must be at least 8 characters long.")
                .MaximumLength(100)
                .WithMessage("New password cannot exceed 100 characters.")
                .NotEqual(command => command.Password.CurrentPassword)
                .WithMessage("New password must be different from the current password.");

            RuleFor(command => command.Password.ConfirmNewPassword)
                .NotEmpty()
                .WithMessage("New password confirmation is required.")
                .Equal(command => command.Password.NewPassword)
                .WithMessage("New password and confirmation password must match.");
        });
    }
}
