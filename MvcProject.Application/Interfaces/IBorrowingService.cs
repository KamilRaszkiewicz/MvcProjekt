using MvcProject.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IBorrowingService
    {
        Task<BaseResponse> BorrowBasketAsync(int usersId, CancellationToken ct);
        Task<BaseResponse> ReturnBooksAsync(List<int> booksIds, int usersId, CancellationToken ct);
    }
}
