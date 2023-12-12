using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MvcProject.Application.Dto;
using MvcProject.Application.Interfaces;
using MvcProject.Infrastructure.Extensions;
using MvcProject.Infrastructure.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Services
{
    internal class FileService : IFileService
    {
        private readonly FileOptions _options;

        public FileService(IOptions<FileOptions> options)
        {
            _options = options.Value;   
        }

        public Task<bool> DeleteFileAsync(IFormFile file, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<SaveFileResponse> SaveFileAsync(IFormFile file, CancellationToken ct)
        {
            var result = new SaveFileResponse();


            if (!_options.ImageAcceptedContentTypes.ContainsKey(file.ContentType))
            {
                result.Status = 1;
                result.Error = "Invalid content type.";

                return result;
            }

            if (file.Length > _options.MaxBytesfileSize)
            {
                result.Status = 2;
                result.Error = "File too big.";

                return result;
            }

            var name = NamesHelper.GetDateSignedName() + _options.ImageAcceptedContentTypes[file.ContentType];

            try
            {
                using (var readStream = file.OpenReadStream())
                {
                    using (var writeStream = new System.IO.StreamWriter(_options.WebRootPath + "\\" + _options.ImagesFolder + "\\" + name))
                    {
                        await readStream.CopyToAsync(writeStream.BaseStream, ct);
                    }
                }
            }
            catch(Exception e)
            {
                result.Status = -1;
            }

            result.Name = name;
            return result;
        }
    }
}
