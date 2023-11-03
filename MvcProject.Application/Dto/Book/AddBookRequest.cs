using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Book
{
    public class AddBookRequest
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string DescriptionShort { get; set; }
        public string DescriptionLong { get; set; }
        public IFormFile CoverImage { get; set; }
        public int Quantity { get; set; }

        public IList<AddBookContentsRequest> Contents { get; set; }
        public IList<int> AuthorIds { get; set; }
        public int CategoryId { get; set; }
    }

    public class AddBookContentsRequest
    {
        public int Page { get; set; }
        public string Content { get; set; }
    }
}
