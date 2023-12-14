using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Borrowing
{
    public class BaseBorrowingDto
    {
        public int BorrowingsId { get; set; }
        public int BooksId { get; set; }
        public string BooksTitle { get; set; }
        public string BooksISBN { get; set; }
        public List<string> BooksAuthors { get; set; }
        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
