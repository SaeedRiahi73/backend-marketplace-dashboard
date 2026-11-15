namespace Task_Application.Models.Product;

public sealed class ProductSummaryReadModel
{
    public int TotalCount { get; init; }
    public int ActiveCount { get; init; }
    public int DeActiveCount { get; init; }
    public int OutOfStockCount { get; init; }
}
