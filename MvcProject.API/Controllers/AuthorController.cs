using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Author;
using MvcProject.Application.Dto.Borrowing;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Interfaces;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/author")]
    public class AuthorController : ApiController
    {
        private readonly IAuthorService _authorService;

        public AuthorController(
            IAuthorService authorService
            )
        {
            _authorService = authorService;
        }

        /// <summary>
        /// Creates new Author. Employee Role required
        /// </summary>
        [HttpPost("")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<CreateAuthorResponse> PostAuthor([FromBody] CreateAuthorRequest req, CancellationToken ct)
        {
            var res = await _authorService.CreateAuthorAsync(req, ct);

            if (res.Status != 0)
            {
                if (res.Status == -1)
                    HttpContext.Response.StatusCode = 500;
                else
                    HttpContext.Response.StatusCode = 400;
            }

            return res;
        }

        /// <summary>
        /// Gets list of Authors.
        /// </summary>
        [AllowAnonymous]
        [HttpGet("")]
        public List<GetAuthorResponse> GetAuthors()
        {
            return  _authorService.GetAuthors();
        }
    }
}
