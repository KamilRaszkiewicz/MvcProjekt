using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Borrowing;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Reflection.Metadata.BlobBuilder;

namespace MvcProject.Application.Services
{
    internal class BorrowingService : IBorrowingService
    {
        private readonly IRepository<Book> _booksRepository;
        private readonly IRepository<BookBorrowing> _borrowingRepository;
        private readonly IRepository<BookBasket> _basketRepository;
        private readonly IRepository<Wish> _wishlistRepository;

        private readonly IPaginationService _paginationService;
        private readonly IBorrowForWishListService _borrowForWishListService;

        public BorrowingService(
            IRepository<Book> booksRepository,
            IRepository<BookBorrowing> borrowingRepository,
            IRepository<BookBasket> basketRepository,
            IRepository<Wish> wishlistRepository,
            IPaginationService paginationService,
            IBorrowForWishListService borrowForWishListService
            )
        {
            _booksRepository = booksRepository;
            _borrowingRepository = borrowingRepository;
            _basketRepository = basketRepository;
            _wishlistRepository = wishlistRepository;

            _paginationService = paginationService;
            _borrowForWishListService = borrowForWishListService;
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

        public GetAllBorrowingsResponse GetAllBorrowings(PaginationBase pagination)
        {
            var result = new GetAllBorrowingsResponse();

            try
            {
                var borrowings = _borrowingRepository
                    .GetAll(x => x.Book.Authors, x => x.User);

                var paginated = _paginationService.GetPaginated(
                        borrowings,
                        x => x.Id,
                        pagination.Page ?? 1,
                        pagination.Size,
                        true
                    );

                result.Borrowings = paginated.Select(x => new ExtendedBorrowingDto
                {
                    BorrowingsId = x.Id,
                    BooksId = x.Book.Id,
                    UsersId = x.UsersId,
                    Email = x.User.Email,
                    BooksTitle = x.Book.Title,
                    BooksISBN = x.Book.ISBN,
                    BooksAuthors = x.Book.Authors.Select(y => y.Name + ' ' + y.LastName).ToList(),
                    BorrowedAt = x.BorrowedAt,
                    ReturnedAt = x.ReturnedAt
                })
                .ToList();
            }
            catch (Exception e)
            {
                result.Status = -1;
            }

            return result;
        }

        public GetUsersBorrowingsResponse GetUsersBorrowings(int usersId)
        {
            var result = new GetUsersBorrowingsResponse();

            try
            {

                result.Borrowings = _borrowingRepository
                    .GetAll(x => x.Book.Authors)
                    .Where(x => x.UsersId == usersId)
                    .Select(x => new BaseBorrowingDto
                    {
                        BorrowingsId = x.Id,
                        BooksId = x.BooksId,
                        BooksTitle = x.Book.Title,
                        BooksISBN = x.Book.ISBN,
                        BooksAuthors =  x.Book.Authors.Select(y => y.Name + ' ' + y.LastName).ToList(),
                        BorrowedAt = x.BorrowedAt,
                        ReturnedAt = x.ReturnedAt
                    })
                    .ToList();

            }
            catch (Exception e)
            {
                result.Status = -1;
            }

            return result;
        }

        public async Task<BaseResponse> ReturnBooksAsync(List<int> borrowingIds, CancellationToken ct)
        {
            var result = new BaseResponse();


            try
            {
                var borrowingsToReturn = _borrowingRepository.GetAll(x => x.Book)
                    .Where(x => borrowingIds.Contains(x.Id) && x.ReturnedAt == null).ToArray();

                var returnedBorrowings = borrowingsToReturn.Select(x =>
                {
                    x.ReturnedAt = DateTime.Now;
                    return x;
                });

                await _borrowingRepository.UpdateRangeAsync(ct, returnedBorrowings.ToArray());

                var returnedBooks = borrowingsToReturn
                    .GroupBy(x => x.Book)
                    .Select(x =>
                    {
                        var count = x.Count();
                        x.Key.Quantity = x.Key.Quantity == 0 ? 0 : x.Key.Quantity + count;
                        return new
                        {
                            x.Key,
                            count
                        };
                    }).ToArray();

                await _booksRepository.UpdateRangeAsync(ct,
                    returnedBooks.Select(x => x.Key).Where(x => x.Quantity != 0).ToArray());

                var booksToBeBorrowedForWishers = returnedBooks.Where(x => x.Key.Quantity == 0).Select(x => (x.Key, x.count)).ToList();

                await _borrowForWishListService.BorrowForFirstUserOnWishlistAsync(booksToBeBorrowedForWishers, ct);
            } 
            catch(Exception e)
            {
                result.Status = -1;
            }

            return result;
        }
    }
}
