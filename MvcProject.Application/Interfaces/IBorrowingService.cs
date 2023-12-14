using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Borrowing;
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
        Task<BaseResponse> ReturnBooksAsync(List<int> borrowingIds, CancellationToken ct);
        GetUsersBorrowingsResponse GetUsersBorrowings(int usersId);
        GetAllBorrowingsResponse GetAllBorrowings(PaginationBase pagination);
    }
}
