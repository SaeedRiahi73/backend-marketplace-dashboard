using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces;
using Task_Application.Contracts.Interfaces.Products;
using Task_Application.Contracts.Interfaces.Services;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Dtos.Product;
using Task_Application.Dtos.Security;
using Task_Application.Enums;
using Task_Application.Features.Products.Requests.Commands;
using Task_Domain.Entities;

namespace Task_Application.Features.Products.Handler.Commands
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommandRequest, ResultInfo<Guid>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IProductRepository _productRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(
            ICurrentUserService currentUserService,
            IProductRepository productRepository,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork)
        {
            _currentUserService = currentUserService;
            _productRepository = productRepository;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultInfo<Guid>> Handle(CreateProductCommandRequest request, CancellationToken cancellationToken)
        {
            Guid? userId = _currentUserService.UserId;

            if (userId is null || userId == Guid.Empty)
                return ResultInfo<Guid>.Failure(
                    ["The user is not authenticated."],
                    status: ResultStatus.Unauthorized);

            // ذخیره فایل در پوشه wwwroot/product
            string imageUrl = string.Empty;
            if (request.CreateProduct.ImageFile != null && request.CreateProduct.ImageFile.Length > 0)
            {
                imageUrl = await _fileStorageService.SaveFileAsync(request.CreateProduct.ImageFile, "product", cancellationToken);
            }
            CreateProductDto dto = request.CreateProduct;

            Product product = new Product(
                dto.Name,
                dto.Description,
                dto.Price,
                dto.Quantity,
                dto.Status,
                userId.Value,
                imageUrl
             );

            await _productRepository.AddAsync(
                product,
                cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ResultInfo<Guid>.Success(
                product.Id,
                "product created successfully",
                ResultStatus.Created);
        }
    }
}
