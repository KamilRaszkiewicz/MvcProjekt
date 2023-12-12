using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.User
{
    public class UserAuthenticationResponse
    {
        public bool IsSuccessful { get; set; }
        public IEnumerable<string> Errors { get; set; }
        public string? Token { get; set; }
        public DateTime? ExpiresAt { get; set; }
        public IList<string>? Roles { get; set; }
    }
}
