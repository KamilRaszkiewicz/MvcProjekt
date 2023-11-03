using Microsoft.EntityFrameworkCore;
using MvcProject.Application.Dto.Category;
using MvcProject.Application.Helpers;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Models;
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
        private readonly IRepository<Category> _repository;

        public CategoryService(IRepository<Category> repository)
        {
            _repository = repository;
        }

        public ICollection<CategoryTreeResponse> GetCategoryTree()
        {
            return _repository
                .GetAll()
                .Include(x => x.ParentCategory)
                .ToTree(x => x.ParentCategory)
                .ToPoco<CategoryTreeResponse>((item, children) => new CategoryTreeResponse
                {
                    Id = item == null ? null : item.Id,
                    Name = item == null ? null : item.Name,
                    Children = children,
                }).Children;
        }
    }
}
