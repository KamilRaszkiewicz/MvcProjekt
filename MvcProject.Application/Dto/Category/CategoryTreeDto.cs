﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Dto.Category
{
    public class CategoryTreeDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public ICollection<CategoryTreeDto> Children { get; set; }
    }
}
