using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Author
{
    public class CreateAuthorResponse: BaseResponse
    {
        public int? AuthorsId { get; set; }
    }
}
