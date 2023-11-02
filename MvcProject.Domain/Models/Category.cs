using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public class Category: BaseEntity
    {
        public string Name { get; set; }
        public Category? ParentCategory { get; set; }
        public IList<Book> Books { get; set; }
    }
}
