using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Dtos.Base;
using Tasks_Domain.Enums;

namespace Task_Application.Dtos.Product
{
    public class UpdateProductDto:BaseDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public StatusEnum Status { get; set; }
        public IFormFile? ImageFile { get; set; }
    }
}
