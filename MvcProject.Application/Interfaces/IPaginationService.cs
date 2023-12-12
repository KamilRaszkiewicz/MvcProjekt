using MvcProject.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IPaginationService
    {
        public IEnumerable<T> GetPaginated<T>(IQueryable<T> entities, Expression<Func<T, object>> expression, int? page, int? pageSize, bool? sortAsc);
    };
}
