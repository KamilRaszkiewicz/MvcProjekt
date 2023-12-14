using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Book;
using MvcProject.Application.Dto.Search;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ApiController
    {
        private readonly ISearchService _searchService;

        public SearchController(
            ISearchService searchService
            )
        {
            _searchService = searchService;
        }


        /// <summary>
        /// Gets paginated list of searches. User role required.
        /// </summary>
        [HttpPost("")]
        [Authorize(Roles = "User")]
        public List<SearchItemResponse> GetSearchesPaginated([FromQuery] PaginationBase pagination)
        {
            var res = _searchService.GetUsersSearchesPaginated((int)UserContext.Id!, pagination);

            return res;
        }
    }
}