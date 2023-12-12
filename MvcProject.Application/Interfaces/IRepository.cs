using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IRepository<T> where T: class
    {
        IQueryable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IQueryable<T> GetAll();
        Task AddAsync(T entity, CancellationToken ct);
        Task AddRangeAsync(CancellationToken ct, params T[] entity);
        Task UpdateAsync(T entity, CancellationToken ct);
        Task UpdateRangeAsync(CancellationToken ct, params T[] entity);
        Task DeleteAsync(CancellationToken ct, T entity);
        Task DeleteRangeAsync(CancellationToken ct, params T[] entity);
    }
}
