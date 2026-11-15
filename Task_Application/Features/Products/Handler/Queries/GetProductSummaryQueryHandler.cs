using AutoMapper;
using MediatR;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Products;
using Task_Application.Dtos.Product;
using Task_Application.Features.Products.Requests.Queries;
using Task_Application.Models.Product;

namespace Task_Application.Features.Products.Handler.Queries;

public sealed class GetProductSummaryQueryHandler
    : IRequestHandler<
        GetProductSummaryQueryRequest,
        ResultInfo<ProductSummaryDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductSummaryQueryHandler(
        IProductRepository productRepository,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ResultInfo<ProductSummaryDto>> Handle(
        GetProductSummaryQueryRequest request,
        CancellationToken cancellationToken)
    {
        ProductSummaryReadModel summary =
            await _productRepository.GetProductSummaryAsync(cancellationToken);

        ProductSummaryDto response =
            _mapper.Map<ProductSummaryDto>(summary);

        return ResultInfo<ProductSummaryDto>.Success(
            response,
            "Product summary retrieved successfully.");
    }
}
