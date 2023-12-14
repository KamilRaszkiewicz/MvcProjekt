using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Borrowing
{
    public class GetAllBorrowingsResponse: BaseResponse
    {
        public List<ExtendedBorrowingDto> Borrowings { get; set; }
    }
}
