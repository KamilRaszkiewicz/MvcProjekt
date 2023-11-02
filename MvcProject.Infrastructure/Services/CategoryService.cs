using Microsoft.EntityFrameworkCore;
using MvcProject.Application.Dto.Category;
using MvcProject.Application.Helpers;
using MvcProject.Application.Interfaces;
using MvcProject.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Infrastructure.Services
{
    internal class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public ICollection<CategoryTreeDto> GetCategoryTree()
        {
            return _context.Category
                .Include(x => x.ParentCategory)
                .ToTree(x => x.ParentCategory)
                .ToPoco<CategoryTreeDto>((item, children) => new CategoryTreeDto
                {
                    Id = item == null ? null : item.Id,
                    Name = item == null ? null : item.Name,
                    Children = children,
                }).Children;
        }
    }
}
