using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Search;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Services
{
    internal class SearchService : ISearchService
    {
        private readonly IRepository<Search> _searchRepository;
        private readonly IPaginationService _paginationService;

        public SearchService(
            IRepository<Search> searchRepository,
            IPaginationService paginationService)
        {
            _searchRepository = searchRepository;
            _paginationService = paginationService;
        }

        public List<SearchItemResponse> GetUsersSearchesPaginated(int usersId, PaginationBase pagination)
        {
            var searches = _searchRepository
                .GetAll()
                .Where(x => x.UsersId == usersId);

            return _paginationService.GetPaginated(
                 searches,
                 x => x.Id,
                 pagination.Page ?? 1,
                 pagination.Size ?? 10,
                 false
                 ).Select(x => new SearchItemResponse
                 {
                     SearchAttribute = x.SearchAttribute,
                     Query = x.Query,
                     SearchedAt = x.SearchedAt,
                 }).ToList();    
        }
    }
}
