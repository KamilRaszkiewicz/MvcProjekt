using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public class Author: BaseEntity
    {
        public string Name { get; set; }
        public string LastName { get; set; }
        public IList<Book> Books { get; set; }
    }
}
