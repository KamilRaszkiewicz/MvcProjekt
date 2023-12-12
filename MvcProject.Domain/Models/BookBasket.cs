using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public class BookBasket
    {
        public IUser User { get; set; }
        public int UsersId { get; set; }

        public Book Book { get; set; }
        public int BooksId { get; set; }
    }
}
