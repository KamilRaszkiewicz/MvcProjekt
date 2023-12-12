using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Interfaces;
using MvcProject.Application.Results;
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


        [HttpPost("")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<BaseResult> AddToBasket([FromBody] int Id, CancellationToken ct)
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

        [HttpGet("")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<BaseResult> GetBasket(CancellationToken ct)
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
    }
}
