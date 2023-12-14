using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Search
{
    public class SearchItemResponse
    {
        public BookSearchAttribute SearchAttribute { get; set; }
        public string Query { get; set; }
        public DateTime SearchedAt { get; set; }
    }
}
