using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Author;
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
    internal class AuthorService : IAuthorService
    {
        private readonly IRepository<Author> _authorRepository;

        public AuthorService(IRepository<Author> authorRepository)
        {
            _authorRepository = authorRepository;
        }

        public async Task<CreateAuthorResponse> CreateAuthorAsync(CreateAuthorRequest req, CancellationToken ct)
        {
            var result = new CreateAuthorResponse();
            try
            {
                var authorExists = _authorRepository.GetAll().Where(x =>
                        x.Name.ToUpper() == req.Name.ToUpper()
                        && x.LastName.ToUpper() == req.LastName.ToUpper()
                    ).Any();

                if(authorExists)
                {
                    result.Status = 1;
                    result.Error = "Author with same name and lastname exists.";

                    return result;
                }
                var author = new Author
                {
                    Name = req.Name,
                    LastName = req.LastName,
                };

                await _authorRepository.AddAsync(author, ct);

                result.AuthorsId = author.Id;
            }
            catch (Exception e)
            {
                result.Status = -1;
            }

            return result;
        }

        public List<GetAuthorResponse> GetAuthors()
        {
            return _authorRepository.GetAll(x => x.Books)
                .OrderByDescending(x => x.Books.Count())
                .Select(x => new GetAuthorResponse
                {
                    Id = x.Id,
                    FullName = x.Name + " " + x.LastName,
                    NumberOfBooks = x.Books.Count()
                }).ToList();
        }
    }
}
