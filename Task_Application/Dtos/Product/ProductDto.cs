using Task_Application.Dtos.Base;
using Tasks_Domain.Enums;

namespace Task_Application.Dtos.Product
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public StatusEnum Status { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string? Image { get; set; }
    }
}
