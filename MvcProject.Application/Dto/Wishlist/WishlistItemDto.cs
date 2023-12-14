using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Wishlist
{
    public class WishlistItemDto
    {
        public int BooksId { get; set; }
        public string BooksTitle { get; set; }
        public string BooksAuthors { get; set; }
        public string BooksISBN { get; set; }
        public string BooksCoverUrl { get; set; }

        public DateTime WishedAt { get; set; }
    }
}
