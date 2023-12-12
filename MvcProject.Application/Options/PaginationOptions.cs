using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Options
{
    public class PaginationOptions
    {
        public int MinPageSize { get; set; } = 5;
        public int MaxPageSize { get; set; } = 50;

        public int DefaultPageSize { get; set; } = 25;
        public bool DefaultSortAsc { get; set; } = true;
    }
}
