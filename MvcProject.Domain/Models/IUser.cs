using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public interface IUser
    {
        public int Id { get; }
        public string Email { get; }
        public bool IsVerified { get; set; }
        public DateTime CreatedAt { get; set; }

        public IList<Search> Searches { get; set; }
        public IList<BookBasket> Basket { get; set; }
        public IList<Wish> WishList { get; set; }
        public IList<BookBorrowing> Borrowings { get; set; }
    }
}
