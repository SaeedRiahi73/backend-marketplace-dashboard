using AutoMapper;
using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Products;
using Task_Application.Dtos.Common;
using Task_Application.Dtos.Product;
using Task_Application.Features.Products.Requests.Queries;
using Task_Application.Models.Product;

namespace Task_Application.Features.Products.Handler.Queries;

public sealed class GetPagedProductsQueryHandler
    : IRequestHandler<
        GetPagedProductsQueryRequest,
        ResultInfo<PagedResultDto<ProductListItemDto>>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetPagedProductsQueryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ResultInfo<PagedResultDto<ProductListItemDto>>> Handle(
        GetPagedProductsQueryRequest request,
        CancellationToken cancellationToken)
    {
        PagedResultDto<ProductListReadModel> productsPage =
            await _productRepository.GetPagedProductsAsync(
                request.Filter,
                cancellationToken);

        List<ProductListItemDto> products =
            _mapper.Map<List<ProductListItemDto>>(productsPage.Items);

        PagedResultDto<ProductListItemDto> response = new()
        {
            Items = products,
            PageNumber = productsPage.PageNumber,
            PageSize = productsPage.PageSize,
            TotalCount = productsPage.TotalCount,
            TotalPages = productsPage.TotalPages
        };

        return ResultInfo<PagedResultDto<ProductListItemDto>>.Success(
            response,
            "Products retrieved successfully.");
    }
}
