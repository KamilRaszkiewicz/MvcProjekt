using MvcProject.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public class Search: BaseEntity
    {
        public virtual IUser User { get; set; }
        public int UsersId { get; set; }
        public string Query { get; set; }
        public DateTime SearchedAt { get; set; }
        public BookSearchAttribute SearchAttribute { get; set; }
    }
}
