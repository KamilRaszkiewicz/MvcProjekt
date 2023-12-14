using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Admin
{
    public class RoleRequest
    {
        [Required]
        public int UsersId { get; set; }
        [Required]
        public Roles Role { get; set; }
    }
}
