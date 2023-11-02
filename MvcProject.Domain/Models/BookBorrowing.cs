using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public class BookBorrowing: BaseEntity
    {
        public IUser User { get; set; }
        public int UsersId { get; set; }

        public Book Book { get; set; }
        public int BooksId { get; set; }

        public DateTime BorrowedAt { get; set; }
        public DateTime? ReturnedAt { get; set; }
    }
}
