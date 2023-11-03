using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto
{
    public class PaginationBase
    {
        public bool? SortAsc { get; set; }
        public int? Page { get; set; }
        public int? Size { get; set; }
    }
}
