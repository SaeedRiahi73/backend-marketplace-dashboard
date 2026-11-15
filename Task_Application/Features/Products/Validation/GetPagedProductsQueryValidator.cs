using FluentValidation;
using Task_Application.Features.Products.Requests.Queries;

namespace Task_Application.Features.Products.Validation;

public sealed class GetPagedProductsQueryValidator
    : AbstractValidator<GetPagedProductsQueryRequest>
{
    public GetPagedProductsQueryValidator()
    {
        RuleFor(request => request.Filter.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page number must be at least 1.");

        RuleFor(request => request.Filter.PageSize)
            .InclusiveBetween(1, 50)
            .WithMessage("Page size must be between 1 and 50.");

        RuleFor(request => request.Filter.Search)
            .MaximumLength(100)
            .WithMessage("Search must not exceed 100 characters.");

        RuleFor(request => request.Filter.Status)
            .IsInEnum()
            .WithMessage("Product status filter is invalid.");

        RuleFor(request => request.Filter.SortBy)
            .IsInEnum()
            .WithMessage("Product sort field is invalid.");

        RuleFor(request => request.Filter.SortOrder)
            .IsInEnum()
            .WithMessage("Product sort order is invalid.");
    }
}
