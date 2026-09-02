using AutoMapper;
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
using Task_Application.Enums;
using Task_Application.Features.Products.Requests.Commands;
using Task_Domain.Entities;

namespace Task_Application.Features.Products.Handler.Commands
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommandRequest, ResultInfo<Unit>>
    {
        private readonly IProductRepository _productRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileStorageService _fileStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            ICurrentUserService currentUserService,
            IFileStorageService fileStorageService,
            IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _currentUserService = currentUserService;
            _fileStorageService = fileStorageService;
            _unitOfWork = unitOfWork;
        }
        public async Task<ResultInfo<Unit>> Handle(UpdateProductCommandRequest request, CancellationToken cancellationToken)
        {

            Guid? userId = _currentUserService.UserId;

            if (userId is null || userId == Guid.Empty)
                return ResultInfo<Unit>.Failure(
                    ["The user is not authenticated."],
                    status: ResultStatus.Unauthorized);

            // 1. پیدا کردن محصول موجود در دیتابیس
            var product = await _productRepository.GetByIdAsync(
                request.updateProductDto.Id,
                cancellationToken);

            if (product == null)
                return ResultInfo<Unit>.Failure(
                    ["Product not found."],
                    status: ResultStatus.NotFound);

            // 2.  بررسی اینکه آیا فقط سازنده محصول می‌تواند آن را ویرایش کند؟
            // if (product.CreatedBy != userId.Value)
            //     return ResultInfo<uint>.Failure(["You do not have permission to update this product."]);
            var newImageUrl = product.Image;

            if (
                request.updateProductDto.ImageFile != null &&
                request.updateProductDto.ImageFile.Length > 0
            )
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    _fileStorageService.DeleteFile(product.Image);
                }

                newImageUrl = await _fileStorageService.SaveFileAsync(
                    request.updateProductDto.ImageFile,
                    "product",
                    cancellationToken
                );
            }
            else if (request.updateProductDto.RemoveImage)
            {
                if (!string.IsNullOrEmpty(product.Image))
                {
                    _fileStorageService.DeleteFile(product.Image);
                }

                newImageUrl = null;
            }

            UpdateProductDto productDto = request.updateProductDto;

            product.Update(
                productDto.Name,
                productDto.Description,
                productDto.Price,
                productDto.Quantity,
                productDto.Status,
                newImageUrl
                );

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return ResultInfo<Unit>.Success(Unit.Value, "Product updated successfully.");

        }
    }
}
