﻿using Microsoft.AspNetCore.Identity;
using MvcProject.Domain;
using MvcProject.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Identity
{
    public class ApplicationUser : IdentityUser<int>, IUser
    {
        public bool IsVerified { get; set; }

        public DateTime CreatedAt { get; set; }

        public IList<Search> Searches { get; set; }
        public IList<BookBasket> Basket { get; set; }
        public IList<Wish> WishList { get; set; }
        public IList<BookBorrowing> Borrowings { get; set; }
    }
}
