using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Book;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
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
        [HttpGet]
        [Route("{id}")]
        public IActionResult GetBook()
        {
            return Ok("Book");
        }


        /// <summary>
        /// Get books in short format by search attribute (optional), with pagination (also optional)
        /// </summary>
        [HttpGet]
        [Route("")]
        public ActionResult<IEnumerable<GetBookShortResponse>> GetBooks(
            [FromQuery] GetBooksRequest req,
            [FromQuery] PaginationRequest<BookSortAttribute> pagination
            )
        {
            try
            {
                return Ok(_bookService.GetShortBooks(req, pagination));
            } 
            catch(Exception e)
            {
                _logger.LogError("Exception thrown at {0}: {1}", nameof(GetBooks), e);

                return BadRequest();
            }
        }


        /// <summary>
        /// Add book
        /// </summary>
        [HttpPost]
        [Route("")]
        public IActionResult AddBook([FromBody] AddBookRequest dto)
        {
            return Ok();
        }

        /// <summary>
        /// Patch book by id
        /// </summary>
        [HttpPatch]
        [Route("{id}")]
        public IActionResult PatchBook(int id, [FromBody] PatchBookRequest dto)
        {
            return Ok("Book");
        }

        /// <summary>
        /// Delete book by id
        /// </summary>
        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeleteBook()
        {
            return Ok("Book");
        }


    }
}
