using Task_Application.Dtos.Base;

namespace Task_Application.Dtos.Product;

public sealed class ProductListItemDto : BaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public int StatusId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Created { get; set; }
    public string? Image { get; set; }
}
