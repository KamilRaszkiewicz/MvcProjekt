using Microsoft.AspNetCore.Identity;
using MvcProject.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<int>
    {
        public bool IsVerified { get; set; }
    }
}
