using Task_Application.Enums;

namespace Task_Application.Dtos.Product;

public sealed class GetProductsFilterDto
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Search { get; set; }
    public ProductFilterStatus Status { get; set; } = ProductFilterStatus.All;
    public ProductSortBy SortBy { get; set; } = ProductSortBy.Created;
    public ProductSortOrder SortOrder { get; set; } = ProductSortOrder.Desc;
}
