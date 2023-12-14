using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Wishlist;
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
    public class WishlistService : IWishlistService
    {
        private readonly IRepository<Wish> _wishlistRepository;
        private readonly IRepository<Book> _booksRepostiory;

        public WishlistService(
            IRepository<Wish> wishlistRepository,
            IRepository<Book> booksRepository
            )
        {
            _wishlistRepository = wishlistRepository;
            _booksRepostiory = booksRepository;
        }

        public async Task<BaseResponse> AddUsersWishlistAsync(int usersId, int booksId, CancellationToken ct)
        {
            var response = new BaseResponse();

            try
            {
                var bookAndWishes = _booksRepostiory
                    .GetAll(x => x.Wishes.Where(x => x.UsersId == usersId))
                    .Where(x => x.Id == booksId)
                    .FirstOrDefault();


                if (bookAndWishes == null)
                {
                    response.Status = 1;
                    response.Error = "The book does not exist.";

                    return response;
                }

                if(bookAndWishes.Quantity > 0)
                {
                    response.Status = 2;
                    response.Error = "The book is available, you can not wish it.";

                    return response;
                }

                if (bookAndWishes.Wishes.Any())
                {
                    response.Status = 3;
                    response.Error = "The book is already wished by you.";

                    return response;
                }


                var bookExists = _wishlistRepository
                    .GetAll()
                    .Where(x => x.UsersId == usersId && x.BooksId == booksId)
                    .FirstOrDefault();


                await _wishlistRepository.AddAsync(new Wish()
                {
                    BooksId = booksId,
                    UsersId = usersId,
                    WishedAt = DateTime.Now
                }, ct);
            }
            catch (Exception e)
            {
                response.Status = -1;
            }

            return response;
        }

        public async Task<BaseResponse> DeleteUsersWishlistAsync(int usersId, List<int>? booksIds, CancellationToken ct)
        {
            var response = new BaseResponse();

            try
            {
                if(booksIds == null)
                {
                    await _wishlistRepository.DeleteRangeAsync(ct,
                        _wishlistRepository.GetAll().Where(x => x.UsersId == usersId).ToArray()
                        );
                } 
                else
                {
                    await _wishlistRepository.DeleteRangeAsync(ct,
                        _wishlistRepository.GetAll().Where(x => x.UsersId == usersId && booksIds.Contains(x.BooksId)).ToArray()
                        );
                }
            }
            catch( Exception e)
            {
                response.Status = -1;
            }

            return response;
        }

        public GetWishlistResponse GetUserWishlist(int usersId)
        {
            var result = new GetWishlistResponse();

            try
            {
                result.Wishlist = _wishlistRepository.GetAll(x => x.Book.Authors).Where(x => x.UsersId == usersId).Select(
                    x => new WishlistItemDto
                    {
                        BooksId = x.BooksId,
                        BooksTitle = x.Book.Title,
                        BooksISBN = x.Book.ISBN,
                        BooksCoverUrl = "/img/" + x.Book.CoverImageFileName,
                        BooksAuthors = string.Join(", ", x.Book.Authors.Select(y => string.Concat(y.Name, " ", y.LastName))),
                        WishedAt = x.WishedAt
                    }).ToList();
            }
            catch(Exception e)
            {
                result.Status = -1;
            }

            return result;
        }
    }
}
