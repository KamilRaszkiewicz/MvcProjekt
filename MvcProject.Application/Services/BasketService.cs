using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Book;
using MvcProject.Application.Dto.Book.SubDtos;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Services
{
    internal class BasketService : IBasketService
    {
        private readonly IRepository<Book> _booksRepository;
        private readonly IRepository<BookBasket> _basketRepository;

        public BasketService(
            IRepository<Book> booksRepository,
            IRepository<BookBasket> basketRepository)
        {
            _booksRepository = booksRepository;
            _basketRepository = basketRepository;
        }
        public async Task<BaseResponse> AddToBasketAsync(int bookId, int usersId, CancellationToken ct)
        {
            var result = new BaseResponse();

            var book = _booksRepository
                .GetAll(x => x.Borrowings.Where(x => x.UsersId == usersId && x.ReturnedAt == null), x => x.Baskets.Where(x => x.UsersId == usersId))
                .FirstOrDefault(x => x.Id == bookId);

            if (book == null)
            {
                result.Status = 1;
                result.Error = "Book does not exist.";

                return result;
            }

            if (book.Quantity < 1)
            {
                result.Status = 2;
                result.Error = "Book is not available (quantity is 0).";

                return result;
            }

            if (book.Borrowings.Any())
            {
                result.Status = 3;
                result.Error = "Book is already borrowed bo you.";

                return result;
            }

            if (book.Baskets.Any())
            {
                result.Status = 4;
                result.Error = "Book is already in your basket.";

                return result;
            }

            try
            {
                await _basketRepository.AddAsync(new BookBasket()
                {
                    BooksId = bookId,
                    UsersId = usersId,
                }, ct);
            }
            catch (Exception e)
            {
                result.Status = -1;
            }

            return result;
        }

        public List<GetBookShortResponse> GetBasket(int usersId)
        {
            return _booksRepository
                .GetAll(x => x.Category, x => x.Authors)
                .Where(x => x.Baskets.Any(y => y.UsersId == usersId))
                .Select(x => new GetBookShortResponse
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
                }).ToList();
        }

        public BaseResponse DeleteFromBasketAsync(List<int>? idsToDelete, int usersId, CancellationToken ct)
        {
            var result = new BaseResponse();

            try
            {
                if (idsToDelete == null)
                {
                    _basketRepository.DeleteRangeAsync(
                        ct, _basketRepository.GetAll().Where(x => x.UsersId == usersId).ToArray()
                        );
                }
                else if (idsToDelete.Any())
                {
                    _basketRepository.DeleteRangeAsync(
                        ct, _basketRepository.GetAll().Where(x => x.UsersId == usersId && idsToDelete.Contains(x.BooksId)).ToArray()
                        );
                }
            }
            catch(Exception e)
            {
                result.Status = -1;
            }

            return result;
        }
    }
}
