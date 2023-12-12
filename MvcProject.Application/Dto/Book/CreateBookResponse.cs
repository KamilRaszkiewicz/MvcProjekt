using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MvcProject.Application.Dto;

namespace MvcProject.Application.Dto.Book
{
    public class CreateBookResponse : BaseResponse
    {
        public int BooksId { get; set; }
    }
}
