using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Book;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/book")]
    public class BookController: ApiController
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }
        /// <summary>
        /// Get book in full format by id
        /// </summary>
        [AllowAnonymous]
        [HttpGet]
        [Route("{id}")]
        public ActionResult<GetBookFullResponse> GetBook(int id)
        {
            try
            {
                var book = _bookService.GetFullBookById(id);

                if(book == null)
                    return NotFound();

                return Ok(book);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception thrown at {method}: {ex}", nameof(GetBooks), ex);
                return BadRequest();
            }
        }


        /// <summary>
        /// Get books in short format by search attribute (optional), with pagination (also optional).
        /// </summary>
        [AllowAnonymous]
        [Authorize]
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GetBookShortResponse>> GetBooks(
            [FromQuery] GetBooksRequest req,
            [FromQuery] PaginationRequest<BookSortAttribute> pagination
            )
        {
            try
            {
                return Ok(_bookService.GetShortBooks(req, pagination, UserContext.Id));
            } 
            catch(Exception e)
            {
                _logger.LogError("Exception thrown at {method}: {e}", nameof(GetBooks), e);

                return BadRequest();
            }
        }


        /// <summary>
        /// Add book. Employee role required
        /// </summary>
        [Authorize(Roles = "Employee")]
        [HttpPost]
        [Route("")]
        public async Task<CreateBookResponse> AddBook([FromForm] AddBookRequest dto, CancellationToken ct)
        {
            var res = await _bookService.CreateBookAsync(dto, ct);

            if(res.Status != 0)
            {
                if (res.Status == -1)
                    HttpContext.Response.StatusCode = 500;
                else
                    HttpContext.Response.StatusCode = 400;
            }

            return res;
        }

        /// <summary>
        /// Patch book by id. Employee role required
        /// </summary>
        [Authorize(Roles = "Employee")]
        [HttpPatch]
        [Route("")]
        public async Task<CreateBookResponse> PatchBook([FromForm] PatchBookRequest dto, CancellationToken ct)
        {
            var res =  await _bookService.PatchBookAsync(dto, ct);

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
        /// Delete books by ids. Employee role required
        /// </summary>

        [Authorize(Roles = "Employee")]
        [HttpDelete]
        [Route("")]
        public async Task<BaseResponse> DeleteBooksAsync([FromBody][Required] List<int> booksIds, CancellationToken ct)
        {
            var res = await _bookService.DeleteBooksAsync(booksIds, ct);

            if (res.Status != 0)
            {
                if (res.Status == -1)
                    HttpContext.Response.StatusCode = 500;
                else
                    HttpContext.Response.StatusCode = 400;
            }

            return res;
        }


    }
}
