using MvcProject.Application.Dto.Book;
using MvcProject.Application.Dto;
using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IBookService
    {
        IEnumerable<GetBookShortResponse> GetShortBooks(GetBooksRequest request, PaginationRequest<BookSortAttribute> pagination);
    }
}
