using Microsoft.AspNetCore.Http;
using MvcProject.Application.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface IFileService
    {
        Task<SaveFileResponse> SaveFileAsync(IFormFile file, CancellationToken ct);
        Task<bool> DeleteFileAsync(IFormFile file, CancellationToken ct);

    }
}
