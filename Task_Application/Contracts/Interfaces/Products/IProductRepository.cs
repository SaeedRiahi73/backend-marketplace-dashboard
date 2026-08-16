using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Domain.Entities;

namespace Task_Application.Contracts.Interfaces.Products
{
    public interface IProductRepository : IGenericRepository<Product>
    {
    }
}
