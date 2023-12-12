using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
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

        [HttpPost("returnBook")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<BaseResponse> ReturnBook([FromBody] List<int> bookIds, CancellationToken ct)
        {
            var res = await _borrowingService.ReturnBooksAsync(bookIds, (int)UserContext.Id!, ct);

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
