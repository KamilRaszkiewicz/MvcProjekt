using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto
{
    public class PaginationRequest<T>: PaginationBase where T: Enum
    {
        public T? SortBy { get; set; }
    }
}
