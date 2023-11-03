using Microsoft.EntityFrameworkCore;
using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Book;
using MvcProject.Application.Dto.Book.SubDtos;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Enums;
using MvcProject.Domain.Models;
using MvcProject.Infrastructure.Database;
using MvcProject.Infrastructure.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Services
{
    internal class BookService: IBookService
    {
        private readonly IRepository<Book> _repository;
        private readonly IPaginationService _pagination;

        private readonly List<Expression<Func<Book, object>>> _sorts = new List<Expression<Func<Book, object>>>
        {
            x => x.Title,
            x => x.ReleasedAt,
            x => x.CreatedAt,
        };

        public BookService(IRepository<Book> repository, IPaginationService pagination)
        {
            _repository = repository;
            _pagination = pagination;
        }
        
        public IEnumerable<GetBookShortResponse> GetShortBooks(GetBooksRequest request, PaginationRequest<BookSortAttribute> pagination)
        {
            Expression<Func<Book, object>> sortBy = pagination.SortBy switch
            {
                BookSortAttribute.Title => _sorts[0],
                BookSortAttribute.ReleaseDate =>  _sorts[1],
                BookSortAttribute.AddedDate => _sorts[2],

                _ => _sorts[0]
            };

            var books = _repository
                .GetAll(x => x.Category, x => x.Authors);


            if (request.SearchBy is not null && request.Value is not null)
            {
                Expression<Func<Book, bool>> searchBy = request.SearchBy switch
                {
                    BookSearchAttribute.Title => x => x.Title.Contains((string)request.Value),
                    BookSearchAttribute.ISBN => x => x.ISBN == (string)request.Value,
                    BookSearchAttribute.Author => x => x.Authors.Any(y => y.Id == int.Parse(request.Value)),

                    _ => x => x.Title == (string)request.Value,
                };

                books = books.Where(searchBy);
            }

            var paginated = _pagination.GetPaginated(
                books,
                sortBy,
                pagination.Page ?? 1,
                pagination.Size,
                pagination.SortAsc
                ).ToList();

            return paginated.Select(x => new GetBookShortResponse
            {
                Id = x.Id,
                Title = x.Title,
                ShortDescription = x.DescriptionShort,
                CoverUrl = "/img/" + x.CoverImageFileName,

                Category = new BookInfoCategory
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name,
                },

                Authors = x.Authors.Select(a => new BookInfoAuthor
                {
                    Id = a.Id,
                    DisplayName = a.Name + " " + a.LastName
                })
            });
            

        }
    }
}
