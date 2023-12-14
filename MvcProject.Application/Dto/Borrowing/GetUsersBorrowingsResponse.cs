using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Borrowing
{
    public class GetUsersBorrowingsResponse: BaseResponse
    {
        public List<BaseBorrowingDto> Borrowings { get; set; }
    }
}
