using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public class Book: BaseEntity
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string DescriptionShort { get; set; }
        public string DescriptionLong { get; set; }
        public string CoverImageFileName { get; set; }

        public int Quantity { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ReleasedAt { get; set; }

        public IList<Contents> TableOfContents { get; set; }
        public IList<Author> Authors { get; set; }

        public Category Category { get; set; }
        public int CategoryId { get; set; }
    }
}
