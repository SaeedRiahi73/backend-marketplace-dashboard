using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_Application.Contracts.Interfaces;
using Task_Persistence.Context;

namespace Task_Persistence.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly TaskDbContext _context;

        public GenericRepository(TaskDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(
            T entity,
            CancellationToken cancellationToken = default)
        {
            await _context.Set<T>().AddAsync(
                entity,
                cancellationToken);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>()
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<T?> GetByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<T>().FindAsync(
                [id],
                cancellationToken);
        }
    }
}
