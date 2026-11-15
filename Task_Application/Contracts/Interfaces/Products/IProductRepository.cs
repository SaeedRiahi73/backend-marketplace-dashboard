using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Dtos.Common;
using Task_Application.Dtos.Product;
using Task_Application.Models.Product;
using Task_Domain.Entities;

namespace Task_Application.Contracts.Interfaces.Products
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        Task<PagedResultDto<ProductListReadModel>> GetPagedProductsAsync(
            GetProductsFilterDto filter,
            CancellationToken cancellationToken = default);

        Task<ProductSummaryReadModel> GetProductSummaryAsync(
            CancellationToken cancellationToken = default);
    }
}
