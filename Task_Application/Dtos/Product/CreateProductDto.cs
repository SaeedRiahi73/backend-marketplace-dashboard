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
    public class CreateProductDto
    {
        public string Name { get; init; } = string.Empty;
        public string? Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public int Quantity { get; init; }
        public StatusEnum Status { get; init; }
        public IFormFile? ImageFile { get; set; }
    }
}
