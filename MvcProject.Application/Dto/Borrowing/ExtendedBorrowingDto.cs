using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Borrowing
{
    public class ExtendedBorrowingDto: BaseBorrowingDto
    {
        public int UsersId { get; set; }
        public string Email { get; set; }
    }
}
