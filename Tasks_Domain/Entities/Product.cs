using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Domain.Common;
using Tasks_Domain.Enums;

namespace Task_Domain.Entities
{
    public class Product
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int Quantity { get; private set; }
        public StatusEnum Status { get; private set; }
        public Guid CreatedBy { get; private set; }
        public DateTime Created { get; private set; }
        public string? Image { get; private set; }

        public Product(string name, string description, decimal price, int quantity, StatusEnum status, Guid createdBy, string? image)
        {
            if (price <= 0)
                throw new DomainException("Price must be greater than zero");

            if (quantity < 0)
                throw new DomainException("Quantity must be greater than zero");

            Id = Guid.NewGuid();
            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            Status = status;
            Image = image;
            CreatedBy = createdBy;
            Created = DateTime.UtcNow;
        }

        //  سازنده بدون پارامتر برای EF Core
        private Product() { }
        public void Update(string name, string description, decimal price, int quantity, StatusEnum status, string? image)
        {

            if (price <= 0)
                throw new DomainException("Price must be greater than zero");

            if (quantity < 0)
                throw new DomainException("Quantity must be greater than zero");

            Name = name;
            Description = description;
            Price = price;
            Quantity = quantity;
            Status = status;
            Image = image;
        }
    }
}
