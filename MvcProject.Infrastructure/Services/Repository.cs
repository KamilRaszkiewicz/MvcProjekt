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
    internal class Repository<T> : IRepository<T> where T: class
    {
        private readonly ApplicationDbContext _context;
        private readonly DbSet<T> _set;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            _set = _context.Set<T>();
        }
        public async Task AddAsync(T entity, CancellationToken ct)
        {
            _set.Add(entity);
            await _context.SaveChangesAsync(ct);
        }

        public async Task AddRangeAsync(CancellationToken ct, params T[] entity)
        {
            _set.AddRange(entity);

            await _context.SaveChangesAsync();
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
