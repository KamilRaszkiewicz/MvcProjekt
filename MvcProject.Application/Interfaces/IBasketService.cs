using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Book;
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
        BaseResponse DeleteFromBasketAsync(List<int>? idsToDelete, int usersId, CancellationToken ct);
        List<GetBookShortResponse> GetBasket(int usersId);
    }
}
