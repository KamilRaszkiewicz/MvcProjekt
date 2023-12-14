using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Borrowing
{
    public class ReturnBooksRequest
    {
        public int UsersId { get; set; }
        public int BooksIds { get; set; }
    }
}
