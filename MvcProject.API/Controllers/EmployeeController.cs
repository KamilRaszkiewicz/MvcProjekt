using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/employee")]
    public class EmployeeController : ApiController
    {
        private readonly IBorrowingService _borrowingService;

        public EmployeeController(
            IBorrowingService borrowingService
            )
        {
            _borrowingService = borrowingService;
        }

        /// <summary>
        /// Borrows all books from basket (if books are available)
        /// </summary>
        [HttpGet("GetUsers")]
        [Authorize(Roles = "")]
        public async Task<BaseResponse> GetUsersData(CancellationToken ct)
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

    }
}
