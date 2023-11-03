using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcProject.Application.Dto.Book.SubDtos;

namespace MvcProject.Application.Dto.Book
{
    public class GetBookFullResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string CoverUrl { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
        public int Quantity { get; set; }

        public IEnumerable<BookInfoAuthor> Authors { get; set; }
        public IEnumerable<BookInfoContent> TableOfContents { get; set; }
        public BookInfoCategory Category { get; set; }
    }
}
