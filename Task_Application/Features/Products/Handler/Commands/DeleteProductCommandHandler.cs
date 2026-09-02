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
using Task_Application.Enums;
using Task_Application.Features.Products.Requests.Commands;
using Task_Domain.Entities;

namespace Task_Application.Features.Products.Handler.Commands
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommandRequest, ResultInfo<Unit>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork
            )
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultInfo<Unit>> Handle(DeleteProductCommandRequest request, CancellationToken cancellationToken)
        {
            Guid? userId = _currentUserService.UserId;

            if (userId is null || userId == Guid.Empty)
                return ResultInfo<Unit>.Failure(
                    ["The user is not authenticated."],
                    status: ResultStatus.Unauthorized);

            Product? product = await _productRepository.GetByIdAsync(
                request.productId,
                cancellationToken);

            if (product == null)
                return ResultInfo<Unit>.Failure(
                    ["Product does not exist"],
                    status: ResultStatus.NotFound);

            if (!string.IsNullOrEmpty(product.Image))
                _fileStorageService.DeleteFile(product.Image);

            _productRepository.Delete(product);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ResultInfo<Unit>.Success(Unit.Value, "Product deleted successfully.");
        }
    }
}
