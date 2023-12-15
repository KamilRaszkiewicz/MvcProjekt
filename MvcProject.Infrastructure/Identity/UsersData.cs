using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Identity
{
    internal class UsersData
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string UsersRole { get; set; }
        public string LastProp   { get; set; }
    }
}
