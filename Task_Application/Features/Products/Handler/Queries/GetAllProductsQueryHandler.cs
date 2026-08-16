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
using Task_Application.Features.Products.Requests.Queries;
using Task_Domain.Entities;

namespace Task_Application.Features.Products.Handler.Queries
{
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQueryRequest, ResultInfo<IEnumerable<ProductDto>>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public GetAllProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ResultInfo<IEnumerable<ProductDto>>> Handle(GetAllProductsQueryRequest request, CancellationToken cancellationToken)
        {
            IEnumerable<Product> products = await _productRepository.GetAllAsync();

            if (products == null || !products.Any())
                return ResultInfo<IEnumerable<ProductDto>>.Failure(["No products found."]);

            IEnumerable<ProductDto> productsDto = _mapper.Map<IEnumerable<ProductDto>>(products);

            return ResultInfo<IEnumerable<ProductDto>>.Success(productsDto, "successed");
        }
    }
}
