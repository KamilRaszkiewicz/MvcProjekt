using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Book
{
    public class PatchBookRequest
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? ISBN { get; set; }
        public string? DescriptionShort { get; set; }
        public string? DescriptionLong { get; set; }
        public IFormFile? CoverImage { get; set; }
        public int? Quantity { get; set; }

        public IList<PatchBookContentsRequest>? Contents { get; set; }
        public IList<int>? AuthorIds { get; set; }
        public int? CategoryId { get; set; }
    }

    public class PatchBookContentsRequest
    {
        public int? ContentsId { get; set; }
        public int Page { get; set; }
        public string Content { get; set; }
    }
}
