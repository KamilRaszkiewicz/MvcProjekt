﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Domain.Models
{
    public class Wish
    {
        public IUser User { get; set; }
        public Book Book { get; set; }

        public int UsersId { get; set; }
        public int BooksId { get; set; }

        public DateTime WishedAt { get; set; }
    }
}
