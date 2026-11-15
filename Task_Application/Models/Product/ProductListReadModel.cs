using Tasks_Domain.Enums;

namespace Task_Application.Models.Product;

public sealed class ProductListReadModel
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Quantity { get; init; }
    public StatusEnum Status { get; init; }
    public DateTime Created { get; init; }
    public string? Image { get; init; }
}
