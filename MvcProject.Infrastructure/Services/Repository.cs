using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Models;
using MvcProject.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Services
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _set;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }
        public async Task<int> AddAsync(T entity, CancellationToken ct)
        {
            _set.Add(entity);
            await _context.SaveChangesAsync(ct);

            return entity.Id;
        }

        public async Task<IEnumerable<int>> AddRangeAsync(CancellationToken ct, params T[] entity)
        {
            _set.AddRange(entity);

            await _context.SaveChangesAsync();

            return entity.Select(x => x.Id).ToArray();
        }

        public async Task DeleteAsync(CancellationToken ct, T entity)
        {
            _set.Remove(entity);

            await _context.SaveChangesAsync(ct);
        }

        public async Task DeleteRangeAsync(CancellationToken ct, params T[] entity)
        {
            _set.RemoveRange(entity);

            await _context.SaveChangesAsync(ct);
        }

        public IQueryable<T> GetAll()
        {
            return _context.Set<T>();
        }
        public IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var firstInclude = includes.First();
            var beginning = _set.Include(firstInclude);

            return includes.Skip(1).Aggregate(beginning, (acc, include) => acc.Include(include));
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct)
        {
            return await _set.FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task<T?> GetByIdAsync(int id, CancellationToken ct, params Expression<Func<T, object>>[] includes)
        {
            var firstInclude = includes.First();
            var beginning = _set.Include(x => firstInclude);

            return await includes
                .Skip(1)
                .Aggregate(beginning, (acc, include) => acc.Include(x => include))
                .FirstOrDefaultAsync(x => x.Id == id, ct);
        }

        public async Task UpdateAsync(T entity, CancellationToken ct)
        {
            _set.Update(entity);

            await _context.SaveChangesAsync(ct);
        }

        public async Task UpdateRangeAsync(CancellationToken ct, params T[] entity)
        {
            _set.UpdateRange(entity);

            await _context.SaveChangesAsync(ct);
        }
    }
}
