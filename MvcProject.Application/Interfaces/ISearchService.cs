using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Search;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface ISearchService
    {
        public List<SearchItemResponse> GetUsersSearchesPaginated(int usersId, PaginationBase pagination);
    }
}
