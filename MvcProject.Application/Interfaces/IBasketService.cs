using MvcProject.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IBasketService
    {
        Task<BaseResponse> AddToBasketAsync(int bookId, int usersId, CancellationToken ct);
        BaseResponse GetBasket(int bookId, int usersId, CancellationToken ct);
        Task<BaseResponse> AddToBasketAsync(int bookId, int usersId, CancellationToken ct);
    }
}
