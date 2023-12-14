using MvcProject.Application.Dto;
using MvcProject.Application.Dto.Author;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IAuthorService
    {
        Task<CreateAuthorResponse> CreateAuthorAsync(CreateAuthorRequest req, CancellationToken ct);
        List<GetAuthorResponse> GetAuthors();
    }
}
