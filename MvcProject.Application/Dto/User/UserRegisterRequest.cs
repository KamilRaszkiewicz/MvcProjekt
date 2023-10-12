using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MvcProject.Application.Dto.User
{
    public class UserRegisterRequest : IValidatableObject
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string RepeatedPassword { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Password != RepeatedPassword)
                yield return new ValidationResult("Hasła muszą być identyczne");
        }
    }
}
