using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Book;
using MvcProject.Application.Dto.Book.SubDtos;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Enums;
using MvcProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Services
{
    internal class BookService: IBookService
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly IRepository<Contents> _contentsRepository;
        private readonly IRepository<Search> _searchRepository;
        private readonly ICategoryService _categoryService;
        private readonly IPaginationService _pagination;
        private readonly IFileService _fileService;
        private readonly IBorrowForWishListService _borrowForWishListService;
        private readonly List<Expression<Func<Book, object>>> _sorts = new()
        {
            x => x.Title,
            x => x.ReleasedAt,
            x => x.CreatedAt,
        };

        public BookService(
            IRepository<Book> bookRepository,
            IRepository<Author> authorRepository,
            IRepository<Contents> contentsRepository,
            IRepository<Search> searchRepository,
            ICategoryService categoryService,
            IFileService fileService,
            IPaginationService pagination, 
            IBorrowForWishListService borrowForWishListService)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _contentsRepository = contentsRepository;
            _searchRepository = searchRepository;
            _categoryService = categoryService;
            _pagination = pagination;
            _fileService = fileService;
            _borrowForWishListService = borrowForWishListService;
        }

        public async Task<CreateBookResponse> CreateBookAsync(AddBookRequest request, CancellationToken ct)
        {
            var createResult = new CreateBookResponse();

            var fileResult = await _fileService.SaveFileAsync(request.CoverImage, ct);
            
            if(fileResult.Status != 0)
            {
                createResult.Status = fileResult.Status;
                createResult.Error = fileResult.Error;

                return createResult;
            }

            //TODO: ADD CREATING AUTHOR HERE!

            var authors = _authorRepository.GetAll().Where(x => request.AuthorIds.Contains(x.Id)).ToList();

            if(authors == null || authors.Count != request.AuthorIds.Count)
            {
                createResult.Status = 3;
                createResult.Error = "Author id is invalid";

                return createResult;
            }

            var book = new Book
            {
                Title = request.Title,
                ISBN = request.ISBN,
                DescriptionShort = request.DescriptionShort,
                DescriptionLong = request.DescriptionLong,
                Quantity = request.Quantity,
                CoverImageFileName = fileResult.Name,

                CategoryId = request.CategoryId,
                Authors = authors,

                TableOfContents = request.Contents.Select(x => new Contents
                {
                    Content = x.Content,
                    Page = x.Page
                }).ToList(),

                CreatedAt = DateTime.Now,
            };
            await _bookRepository.AddAsync(book, ct);

            createResult.BooksId = book.Id;

            return createResult;
        }

        public async Task<BaseResponse> DeleteBooksAsync(List<int> booksIds, CancellationToken ct)
        {
            var response = new BaseResponse();

            try
            {
                await _bookRepository.DeleteRangeAsync(ct,
                    booksIds.Select(x => new Book
                    {
                        Id = x
                    }).ToArray());
            }
            catch(Exception e)
            {
                response.Status = -1;   
            }

            return response;
        }

        public GetBookFullResponse? GetFullBookById(int id)
        {
            var book = _bookRepository.GetAll(x => x.Authors, x => x.Category, x => x.TableOfContents).Where(x => x.Id == id).FirstOrDefault();

            if (book == null) return null;

            return new GetBookFullResponse
            {
                Id = book.Id,
                ISBN = book.ISBN,
                Title = book.Title,
                Quantity = book.Quantity,
                CoverUrl = "/img/" + book.CoverImageFileName,
                ShortDescription = book.DescriptionShort,
                LongDescription = book.DescriptionLong,

                Authors = book.Authors.Select(x => new BookInfoAuthor
                {
                    Id = x.Id,
                    DisplayName = x.Name + " " + x.LastName
                }),

                Category = new BookInfoCategory
                {
                    Id = book.Category.Id,
                    Name = book.Category.Name,
                },

                TableOfContents = book.TableOfContents.Select(x => new BookInfoContent
                {
                    Id = x.Id,
                    Page = x.Page,
                    Content = x.Content,
                })

            };

        }

        public IEnumerable<GetBookShortResponse> GetShortBooks(GetBooksRequest request, PaginationRequest<BookSortAttribute> pagination, int? usersId)
        {
            Expression<Func<Book, object>> sortBy = pagination.SortBy switch
            {
                BookSortAttribute.Title => _sorts[0],
                BookSortAttribute.ReleaseDate =>  _sorts[1],
                BookSortAttribute.AddedDate => _sorts[2],

                _ => _sorts[0]
            };

            var books = _bookRepository
                .GetAll(x => x.Category, x => x.Authors);

            if(request.CategoryId != null)
            {
                var categoryIds = _categoryService.GetDescendantCategoryIds((int)request.CategoryId);

                if(!categoryIds.Any())
                {
                    throw new ArgumentException("Category does not exist");
                }

                books = books.Where(x => categoryIds.Contains(x.CategoryId));
            }


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

            if(usersId != null && request.SearchBy != null)
            {
                _searchRepository.AddAsync(new Search
                {
                    ApplicationUserId = (int)usersId,
                    SearchAttribute = (BookSearchAttribute)request.SearchBy,
                    Query = (string)request.Value,
                    SearchedAt = DateTime.Now,

                }, CancellationToken.None);
            }

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

        public async Task<CreateBookResponse> PatchBookAsync(PatchBookRequest request, CancellationToken ct)
        {
            List<Author>? authors = null;
            SaveFileResponse saveFileResult = null;

            var result = new CreateBookResponse();
            var book = _bookRepository.GetAll(x => x.Authors, x => x.Category, x => x.TableOfContents).Where(x => x.Id == request.Id).FirstOrDefault();


            if (request.CoverImage != null)
            {
                saveFileResult = await _fileService.SaveFileAsync(request.CoverImage, ct);

                if(saveFileResult.Status != 0)
                {
                    result.Status = saveFileResult.Status;
                    result.Error = saveFileResult.Error;

                    return result;
                }
            }

            if (book == null)
            {
                result.Status = 3;
                result.Error = "Book does not exist.";

                return result;
            }

            if (request.Contents != null)
            {
                if (request.Contents.Any(x => x.ContentsId != null
                && book.TableOfContents.Select(y => y.Id).Contains((int)x.ContentsId)
                ))
                {
                    result.Status = 4;
                    result.Error = "Can not edit contents that are not owned by the book.";

                    return result;
                }

                await _contentsRepository.UpdateRangeAsync(
                        ct,
                        book.TableOfContents.Join(
                            request.Contents.Where(x => x.ContentsId != null),
                            x => x.Id,
                            y => y.ContentsId,
                            (x, y) =>
                            {
                                x.Page = y.Page;
                                x.Content = y.Content;

                                return x;
                            }).ToArray());


                foreach (var c in request.Contents)
                    book.TableOfContents.Add(new Contents
                    {
                        Page = c.Page,
                        Content = c.Content
                    });
            }

            if (request.AuthorIds != null)
            {
                var newAuthors = _authorRepository.GetAll().Where(x => request.AuthorIds.Contains(x.Id)).ToList();
                var nonExistentAuthorIds = request.AuthorIds.Except(newAuthors.Select(x => x.Id));

                if (nonExistentAuthorIds.Any())
                {
                    result.Status = 5;
                    result.Error = $"Passed non existent author ids: {string.Join(", ", nonExistentAuthorIds)}";

                    return result;
                }

                authors = newAuthors;
            }


            book.Title = request.Title ?? book.Title;
            book.ISBN = request.ISBN ?? book.ISBN;
            book.DescriptionShort = request.DescriptionShort ?? book.DescriptionShort;
            book.DescriptionLong = request.DescriptionLong ?? book.DescriptionLong;
            book.Quantity = book.Quantity;
            book.ISBN = request.ISBN ?? book.ISBN;
            book.CategoryId = request.CategoryId ?? book.CategoryId;
            book.Quantity = (book.Quantity != 0 && request.Quantity != null) ? (int)request.Quantity : 0;
            book.Authors = authors ?? book.Authors;
            book.CoverImageFileName = saveFileResult?.Name ?? book.CoverImageFileName;


            await _bookRepository.UpdateAsync(book, ct);

            if(book.Quantity == 0 && request.Quantity != null && request.Quantity != 0)
            {
                await _borrowForWishListService.BorrowForFirstUserOnWishlistAsync(new List<(Book book, int maxNrOfUsers)>
                {
                    (book, (int)request.Quantity)
                }, ct);
            }    

            result.BooksId = book.Id;
            return result;
        }
    }
}
