namespace Task_Application.Dtos.Product;

public sealed class ProductSummaryDto
{
    public int TotalCount { get; set; }
    public int ActiveCount { get; set; }
    public int DeActiveCount { get; set; }
    public int OutOfStockCount { get; set; }
}
