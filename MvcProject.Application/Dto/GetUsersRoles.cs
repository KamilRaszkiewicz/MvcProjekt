using MvcProject.Application.Dto.Borrowing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto
{
    public class GetUsersRoles
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
