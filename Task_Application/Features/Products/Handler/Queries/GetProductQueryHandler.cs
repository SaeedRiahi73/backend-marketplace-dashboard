using AutoMapper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Products;
using Task_Application.Dtos.Product;
using Task_Application.Enums;
using Task_Application.Features.Products.Requests.Queries;
using Task_Domain.Entities;

namespace Task_Application.Features.Products.Handler.Queries
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQueryRequest, ResultInfo<ProductDto>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetProductQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ResultInfo<ProductDto>> Handle(GetProductQueryRequest request, CancellationToken cancellationToken)
        {
            Product? product = await _productRepository.GetByIdAsync(
                request.productId,
                cancellationToken);

            if (product == null)
                return ResultInfo<ProductDto>.Failure(
                    ["Product not found."],
                    status: ResultStatus.NotFound);

            ProductDto productDto = _mapper.Map<ProductDto>(product);

            return ResultInfo<ProductDto>.Success(productDto, "successed");
        }
    }
}
