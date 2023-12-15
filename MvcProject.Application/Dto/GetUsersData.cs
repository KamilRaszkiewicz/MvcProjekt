using MvcProject.Application.Dto.Borrowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto
{
    public class GetUsersData: GetUsersRoles
    {
        public List<BaseBorrowingDto> UnreturnedBorrowings { get; set; }
    }
}
