using FluentValidation;
using Task_Application.Features.Users.Requests.Queries;

namespace Task_Application.Features.Users.Validation;

public sealed class GetUsersListQueryValidator : AbstractValidator<GetUsersListQueryRequest>
{
    public GetUsersListQueryValidator()
    {
        RuleFor(request => request.Filter.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(request => request.Filter.PageSize)
            .InclusiveBetween(1, 50)
            .WithMessage("Page size must be between 1 and 50.");

        RuleFor(request => request.Filter.Search)
            .MaximumLength(50)
            .WithMessage("Search must not exceed 50 characters.");

        RuleFor(request => request.Filter.Role)
            .Must(role => role is null || Enum.IsDefined(role.Value))
            .WithMessage("Role is invalid.");

        RuleFor(request => request.Filter.SortOrder)
            .IsInEnum()
            .WithMessage("Sort order is invalid.");
    }
}
