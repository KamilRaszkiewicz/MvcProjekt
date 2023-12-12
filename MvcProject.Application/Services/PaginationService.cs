using Microsoft.Extensions.Options;
using MvcProject.Application.Interfaces;
using MvcProject.Application.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace MvcProject.Application.Services
{
    internal class PaginationService : IPaginationService
    {
        private readonly PaginationOptions _options;


        public PaginationService(IOptions<PaginationOptions> options)
        {
            _options = options.Value;
        }

        public IEnumerable<T> GetPaginated<T>(IQueryable<T> entities, Expression<Func<T, object>> expression, int? page, int? pageSize, bool? sortAsc)
        {
            var finalPageSize = pageSize ?? _options.DefaultPageSize;

            finalPageSize = finalPageSize < _options.MinPageSize
                ? _options.MinPageSize
                : finalPageSize;

            finalPageSize = finalPageSize > _options.MaxPageSize
                ? _options.MaxPageSize
                : finalPageSize;

            var ordered = (sortAsc ?? _options.DefaultSortAsc )
                ? entities.OrderBy(expression)
                : entities.OrderByDescending(expression);

            return ordered
                .Skip(finalPageSize * ((page ?? 1) - 1))
                .Take(finalPageSize);
        }
    }
}
