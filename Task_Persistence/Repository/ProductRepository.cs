using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Contracts.Interfaces.Products;
using Task_Domain.Entities;
using Task_Persistence.Context;

namespace Task_Persistence.Repository
{
    public class ProductRepository : GenericRepository<Product>,IProductRepository
    {
        private readonly TaskDbContext _dbContext;

        public ProductRepository(TaskDbContext dbContext):base(dbContext) 
        {
            _dbContext = dbContext;
        }
    }
}
