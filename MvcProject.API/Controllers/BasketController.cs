using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Book;
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
    [Route("api/basket")]
    public class BasketController : ApiController
    {
        private readonly IBasketService _basketService;

        public BasketController(
            IBasketService basketService
            )
        {
            _basketService = basketService;
        }


        /// <summary>
        /// Adds book to basket. VerifiedUser role required.
        /// </summary>
        [HttpPost("")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<BaseResponse> AddToBasket([FromBody] int Id, CancellationToken ct)
        {
            var res = await _basketService.AddToBasketAsync(Id, (int)UserContext.Id!, ct);

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
        /// Gets books from basket in short format. VerifiedUser role required.
        /// </summary>
        [HttpGet("")]
        [Authorize(Roles = "VerifiedUser")]
        public List<GetBookShortResponse> GetBasket()
        {
            return _basketService.GetBasket((int)UserContext.Id!);
        }

        /// <summary>
        /// Removes list of items from basket. If list not passed, clears whole basket. VerifiedUser role required.
        /// </summary>
        [HttpDelete("")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<BaseResponse> DeleteFromBasket([FromBody] List<int>? idsToDelete, CancellationToken ct)
        {
            var res = _basketService.DeleteFromBasketAsync(idsToDelete, (int)UserContext.Id, ct);

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
