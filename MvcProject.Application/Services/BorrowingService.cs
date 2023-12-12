using MvcProject.Application.Dto;
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
    internal class BorrowingService : IBorrowingService
    {
        private readonly IRepository<Book> _booksRepository;
        private readonly IRepository<BookBorrowing> _borrowingRepository;
        private readonly IRepository<BookBasket> _basketRepository;

        public BorrowingService(
            IRepository<Book> booksRepository,
            IRepository<BookBorrowing> borrowingRepository,
            IRepository<BookBasket> basketRepository
            )
        {
            _booksRepository = booksRepository;
            _borrowingRepository = borrowingRepository;
            _basketRepository = basketRepository;
        }

        public async Task<BaseResponse> BorrowBasketAsync(int usersId, CancellationToken ct)
        {
            var result = new BaseResponse();
            var basket = _basketRepository.GetAll(x => x.Book).Where(x => x.UsersId == usersId).ToList();

            if (!basket.Any())
            {
                result.Status = 1;
                result.Error = "Basket must not be empty to borrow.";

                return result;
            }

            if(basket.Any(x => x.Book.Quantity < 1))
            {
                result.Status = 2;
                result.Error = "At least one of books went out of availability.";

                var basketsOutOfAvailability = basket.Where(x => x.Book.Quantity < 0);

                await _basketRepository.DeleteRangeAsync(ct, basketsOutOfAvailability.ToArray());

                return result;
            }

            try
            {
                await _booksRepository.UpdateRangeAsync(ct, basket.Select(x => {
                    x.Book.Quantity = x.Book.Quantity - 1;
                    return x.Book;
                }).ToArray());

                var borrowings = basket.Select(x => new BookBorrowing
                {
                    UsersId = usersId,
                    BooksId = x.BooksId,
                    BorrowedAt = DateTime.Now,
                });

                await _borrowingRepository.AddRangeAsync(ct, borrowings.ToArray());

                _basketRepository.DeleteRangeAsync(ct, basket.ToArray());
            }
            catch (Exception e)
            {
                result.Status = -1;
            }

            return result;
        }

        public async Task<BaseResponse> ReturnBooksAsync(List<int> booksIds, int usersId, CancellationToken ct)
        {
            var result = new BaseResponse();

            try
            {
                var borrowingsToReturn = _borrowingRepository.GetAll(x => x.Book)
                    .Where(x => x.UsersId == usersId && x.ReturnedAt == null && booksIds.Contains(x.Book.Id)).ToArray();

                var returnedBorrowings = borrowingsToReturn.Select(x =>
                {
                    x.ReturnedAt = DateTime.Now;
                    return x;
                });

                await _borrowingRepository.UpdateRangeAsync(ct, returnedBorrowings.ToArray());

                var returnedBooks = borrowingsToReturn.Select(x =>
                {
                    x.Book.Quantity++;
                    return x.Book;
                });

                await _booksRepository.UpdateRangeAsync(ct, returnedBooks.ToArray());
            } 
            catch(Exception e)
            {
                result.Status = -1;
            }

            return result;
        }
    }
}
