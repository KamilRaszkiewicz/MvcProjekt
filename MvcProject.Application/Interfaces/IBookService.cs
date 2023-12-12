using MvcProject.Application.Dto.Book;
using MvcProject.Application.Dto;
using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace MvcProject.Application.Interfaces
{
    public interface IBookService
    {
        IEnumerable<GetBookShortResponse> GetShortBooks(GetBooksRequest request, PaginationRequest<BookSortAttribute> pagination, int? usersId);
        GetBookFullResponse? GetFullBookById(int id);

        Task<CreateBookResponse> CreateBookAsync(AddBookRequest request, CancellationToken ct);
        Task<CreateBookResponse> PatchBookAsync(PatchBookRequest request, CancellationToken ct);
    }
}
