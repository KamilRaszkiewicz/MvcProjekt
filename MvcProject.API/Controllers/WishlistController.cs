using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Borrowing;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Dto.Wishlist;
using MvcProject.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/wishlist")]
    public class WishlistController : ApiController
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(
            IWishlistService wishlistService
            )
        {
            _wishlistService = wishlistService;
        }


        /// <summary>
        /// Gets list of wishlist items
        /// </summary>
        [HttpGet("")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<GetWishlistResponse> GetWishlist()
        {
            var res = _wishlistService.GetUserWishlist((int)UserContext.Id);

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
        /// Adds item to wishlist
        /// </summary>
        [HttpPost("")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<BaseResponse> PostWishlist([FromBody] int booksId, CancellationToken ct)
        {
            var res = await _wishlistService.AddUsersWishlistAsync((int)UserContext.Id, booksId, ct);

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
        /// Deletes items from wishlist by booksIds. If booksId is null, then whole wishlist is deleted
        /// </summary>
        [HttpDelete("")]
        [Authorize(Roles = "VerifiedUser")]
        public async Task<BaseResponse> DeleteWishlist([FromBody] List<int>? booksIds, CancellationToken ct)
        {
            var res = await _wishlistService.DeleteUsersWishlistAsync((int)UserContext.Id, booksIds, ct);

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
