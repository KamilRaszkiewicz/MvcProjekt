using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
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
    [Route("api/borrowing")]
    public class BorrowingController : ApiController
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingController(
            IBorrowingService borrowingService
            )
        {
            _borrowingService = borrowingService;
        }

        /// <summary>
        /// Borrows all books from basket (if books are available). VerifiedUser role required.
        /// </summary>
        [HttpPost("borrowBasket")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<BaseResponse> BorrowBasket(CancellationToken ct)
        {
            var res = await _borrowingService.BorrowBasketAsync((int)UserContext.Id!, ct);

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
        /// Returns a borrowings by id. Employee role required.
        /// </summary>
        [HttpPost("returnBook")]
        [Authorize(Roles = "Employee")]
        public async Task<BaseResponse> ReturnBook([FromBody][Required]List<int> request, CancellationToken ct)
        {
            var res = await _borrowingService.ReturnBooksAsync(request, ct);

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
        /// Gets a current user borrowings. VerifiedUser role required.
        /// </summary>
        [HttpPost("getBorrowings")]
        [Authorize(Roles = "VerifiedUser")]
        public GetUsersBorrowingsResponse GetBorrowings()
        {
            var res = _borrowingService.GetUsersBorrowings((int)UserContext.Id!);

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
        /// Gets paginated list of all borrowings. Employee role Required.
        /// </summary>
        [HttpGet("all")]
        [Authorize(Roles = "Employee")]
        public GetAllBorrowingsResponse GetAllBorrowings([FromQuery] PaginationBase pagination)
        {
            var res = _borrowingService.GetAllBorrowings(pagination);

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
