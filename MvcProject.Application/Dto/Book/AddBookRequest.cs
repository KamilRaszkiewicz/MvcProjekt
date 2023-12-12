using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Book
{
    public class AddBookRequest : IValidatableObject
    {
        [Required]
        [MaxLength(255)]
        public string Title { get; set; }
        [Required]
        public string ISBN { get; set; }
        [Required]
        [MaxLength(255)]
        public string DescriptionShort { get; set; }
        [Required]
        [MaxLength(8000)]
        public string DescriptionLong { get; set; }
        public IFormFile CoverImage { get; set; }
        [Required]
        public int Quantity { get; set; }

        public IList<AddBookContentsRequest> Contents { get; set; }
        public IList<int> AuthorIds { get; set; }
        [MaxLength(255)]
        public string? AuthorName { get; set; }
        [MaxLength(255)]
        public string? AuthorLastName { get; set; }
        public int CategoryId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ISBN.Length != 10 && ISBN.Length != 13)
                yield return new ValidationResult("ISBN has to be either 10 or 13 characters long");

            if (ISBN.Any(x => !char.IsAsciiDigit(x)))
                yield return new ValidationResult("ISBN has to contain only digits");
            
            if(AuthorIds == null || !AuthorIds.Any())
            {
                if (AuthorName == null || AuthorLastName == null)
                    yield return new ValidationResult("If AuthorIds list is empty, then you need to specify the author data in AuthorName and AuthorLastName field");
            }


        
        }
    }

    public class AddBookContentsRequest
    {
        public int Page { get; set; }
        [Required]
        [MaxLength(255)]
        public string Content { get; set; }
    }
}
