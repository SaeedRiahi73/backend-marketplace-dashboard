using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Common.Responses;
using Task_Application.Contracts.Interfaces.Products;
using Task_Application.Contracts.Interfaces.Services;
using Task_Application.Contracts.Interfaces.Users;
using Task_Application.Features.Products.Requests.Commands;
using Task_Domain.Entities;

namespace Task_Application.Features.Products.Handler.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, ResultInfo<Unit>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;

        public DeleteProductCommandHandler(
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService
            )
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
        }
        public async Task<ResultInfo<Unit>> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            Guid? userId = _currentUserService.UserId;

            if (userId is null || userId == Guid.Empty)
                return ResultInfo<Unit>.Failure(["The user is not authenticated."]);

            Product? product = await _productRepository.GetByIdAsync(request.productId);

            if (product == null)
                return ResultInfo<Unit>.Failure(["Product does not exist"]);

            if (!string.IsNullOrEmpty(product.Image))
                _fileStorageService.DeleteFile(product.Image);

            await _productRepository.DeleteAsync(product);

            return ResultInfo<Unit>.Success(Unit.Value, "Product deleted successfully.");
        }
    }
}
