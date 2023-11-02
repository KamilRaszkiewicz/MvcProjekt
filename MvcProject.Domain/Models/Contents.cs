using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public class Contents: BaseEntity
    {
        public int Page { get; set; }
        public string Content { get; set; }
        public Contents? ParentContents { get; set; }
    }
}
