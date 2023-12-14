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
    internal class BorrowForWishListService : IBorrowForWishListService
    {
        private readonly IRepository<Wish> _wishlistRepository;
        private readonly IRepository<BookBorrowing> _borrowingRepository;
        private readonly IRepository<Book> _bookRepository;

        public BorrowForWishListService(
            IRepository<Wish> wishlistRepository,
            IRepository<BookBorrowing> borrowingRepository,
            IRepository<Book> bookRepository
            )
        {
            _wishlistRepository = wishlistRepository;
            _borrowingRepository = borrowingRepository;
            _bookRepository = bookRepository;
        }

        public async Task BorrowForFirstUserOnWishlistAsync(List<(Book book, int maxNrOfUsers)> books, CancellationToken ct)
        {
            var idsAndNrs = books.Select(x => new 
            { 
                Id = x.book.Id,
                maxNrOfUsers = x.maxNrOfUsers
            }).ToList();

            /*            var wishes = _wishlistRepository
                            .GetAll(x => x.Book)
                            .Where(x => idsAndNrs.Select(x => x.Id).Contains(x.BooksId))
                            .GroupBy(x => x.BooksId)
                            .Select(x => new
                            {
                                BooksId = x.Key,
                                UsersData = x.Select(y => new { y.UsersId, y.WishedAt })
                                            .OrderBy(y => y.WishedAt)
                                            .Take(idsAndNrs.First(z => z.Id == x.Key).maxNrOfUsers),
                            }).ToList();


            */


            var wishes = _wishlistRepository
               .GetAll(x => x.Book)
               .Where(x => books.Select(x => x.book.Id).ToList().Contains(x.BooksId))
               .ToList();

            var wishesToDelete = wishes
                .Join(idsAndNrs, x => x.BooksId, x => x.Id, (a, b) =>  new
                {
                    Wish = a,
                    MaxNrOfUsers = b.maxNrOfUsers,
                }).ToList()
                .GroupBy(x => new { x.Wish.BooksId, x.MaxNrOfUsers })
                .Select(x => new
                {
                    BooksId = x.Key.BooksId,
                    Wishes = x.Select(y => y.Wish)
                                .OrderBy(y => y.WishedAt)
                                .Take(x.Key.MaxNrOfUsers).ToArray(),
                    MaxNrOfUsers = x.Key.MaxNrOfUsers
                }).ToArray();


            await _wishlistRepository.DeleteRangeAsync(
                ct,
                wishesToDelete.SelectMany(x => x.Wishes).ToArray());

            var updatedBooks = wishesToDelete.Join(books, x => x.BooksId, x => x.book.Id, (w, b) =>
            {
                b.book.Quantity = w.MaxNrOfUsers - w.Wishes.Length;

                return b.book;
            }).ToArray();

            await _bookRepository.UpdateRangeAsync(ct, updatedBooks);

            await _borrowingRepository.AddRangeAsync(ct, wishesToDelete.SelectMany(x => x.Wishes.Select(
                y => new BookBorrowing()
                {
                    BooksId = x.BooksId,
                    UsersId = y.UsersId,
                    BorrowedAt = DateTime.Now,
                }).Take(x.MaxNrOfUsers - updatedBooks.First(z => z.Id == x.BooksId).Quantity)).ToArray());
        }
    }
}
