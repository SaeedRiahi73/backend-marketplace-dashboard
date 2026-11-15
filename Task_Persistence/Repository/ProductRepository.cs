using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Task_Application.Contracts.Interfaces.Products;
using Task_Application.Dtos.Common;
using Task_Application.Dtos.Product;
using Task_Application.Enums;
using Task_Application.Models.Product;
using Task_Domain.Entities;
using Task_Persistence.Context;
using Tasks_Domain.Enums;

namespace Task_Persistence.Repository
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        private readonly TaskDbContext _dbContext;

        public ProductRepository(TaskDbContext dbContext):base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<PagedResultDto<ProductListReadModel>> GetPagedProductsAsync(
            GetProductsFilterDto filter,
            CancellationToken cancellationToken = default)
        {
            IQueryable<Product> query = _dbContext.Products
                .AsNoTracking()
                .Where(product => product.Status != StatusEnum.Deleted);

            if (!string.IsNullOrWhiteSpace(filter.Search))
            {
                string search = filter.Search.Trim();

                query = query.Where(product => product.Name.Contains(search));
            }

            query = filter.Status switch
            {
                ProductFilterStatus.Active => query.Where(product =>
                    product.Status == StatusEnum.Active && product.Quantity > 0),

                ProductFilterStatus.DeActive => query.Where(product =>
                    product.Status == StatusEnum.DeActive && product.Quantity > 0),

                // An unavailable product belongs only to OutOfStock,
                // even when its stored status is Active or DeActive.
                ProductFilterStatus.OutOfStock => query.Where(product =>
                    product.Quantity == 0),

                _ => query
            };

            int totalCount = await query.CountAsync(cancellationToken);

            query = ApplySorting(query, filter.SortBy, filter.SortOrder);

            IReadOnlyList<ProductListReadModel> items = await query
                .Skip((filter.PageNumber - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(product => new ProductListReadModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    Quantity = product.Quantity,
                    Status = product.Status,
                    Created = product.Created,
                    Image = product.Image
                })
                .ToListAsync(cancellationToken);

            return new PagedResultDto<ProductListReadModel>
            {
                Items = items,
                PageNumber = filter.PageNumber,
                PageSize = filter.PageSize,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)filter.PageSize)
            };
        }

        public async Task<ProductSummaryReadModel> GetProductSummaryAsync(
            CancellationToken cancellationToken = default)
        {
            ProductSummaryReadModel? summary = await _dbContext.Products
                .AsNoTracking()
                .Where(product => product.Status != StatusEnum.Deleted)
                .GroupBy(product => 1)
                .Select(products => new ProductSummaryReadModel
                {
                    TotalCount = products.Count(),
                    ActiveCount = products.Count(product =>
                        product.Status == StatusEnum.Active && product.Quantity > 0),
                    DeActiveCount = products.Count(product =>
                        product.Status == StatusEnum.DeActive && product.Quantity > 0),

                    // Quantity zero has priority over the stored product status.
                    OutOfStockCount = products.Count(product => product.Quantity == 0)
                })
                .SingleOrDefaultAsync(cancellationToken);

            return summary ?? new ProductSummaryReadModel();
        }

        private static IQueryable<Product> ApplySorting(
            IQueryable<Product> query,
            ProductSortBy sortBy,
            ProductSortOrder sortOrder)
        {
            return (sortBy, sortOrder) switch
            {
                (ProductSortBy.Name, ProductSortOrder.Asc) => query
                    .OrderBy(product => product.Name)
                    .ThenBy(product => product.Id),

                (ProductSortBy.Name, ProductSortOrder.Desc) => query
                    .OrderByDescending(product => product.Name)
                    .ThenBy(product => product.Id),

                (ProductSortBy.Price, ProductSortOrder.Asc) => query
                    .OrderBy(product => product.Price)
                    .ThenBy(product => product.Id),

                (ProductSortBy.Price, ProductSortOrder.Desc) => query
                    .OrderByDescending(product => product.Price)
                    .ThenBy(product => product.Id),

                (ProductSortBy.Quantity, ProductSortOrder.Asc) => query
                    .OrderBy(product => product.Quantity)
                    .ThenBy(product => product.Id),

                (ProductSortBy.Quantity, ProductSortOrder.Desc) => query
                    .OrderByDescending(product => product.Quantity)
                    .ThenBy(product => product.Id),

                (ProductSortBy.Created, ProductSortOrder.Asc) => query
                    .OrderBy(product => product.Created)
                    .ThenBy(product => product.Id),

                _ => query
                    .OrderByDescending(product => product.Created)
                    .ThenBy(product => product.Id)
            };
        }
    }
}
