using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Book
{
    public class GetBooksRequest
    {
        public BookSearchAttribute? SearchBy { get; set; }
        public string? Value { get; set; }
    }
}
