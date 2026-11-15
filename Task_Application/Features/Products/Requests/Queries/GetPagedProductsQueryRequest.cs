using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Dtos.Common;
using Task_Application.Dtos.Product;

namespace Task_Application.Features.Products.Requests.Queries;

public sealed record GetPagedProductsQueryRequest(GetProductsFilterDto Filter)
    : IRequest<ResultInfo<PagedResultDto<ProductListItemDto>>>;
