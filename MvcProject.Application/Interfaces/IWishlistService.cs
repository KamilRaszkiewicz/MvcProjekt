using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Wishlist;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IWishlistService
    {
        GetWishlistResponse GetUserWishlist(int usersId);
        Task<BaseResponse> AddUsersWishlistAsync(int usersId, int booksId, CancellationToken ct);
        Task<BaseResponse> DeleteUsersWishlistAsync(int usersId, List<int>? booksIds, CancellationToken ct);
    }
}
