using MvcProject.Application.Dto.Category;
using MvcProject.Application.Helpers;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Models;
using System.Collections.Generic;

namespace MvcProject.Application.Services
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
                .GetAll(x => x.ParentCategory)
                .ToTree(x => x.ParentCategory)
                .ToPoco<CategoryTreeResponse>((item, children) => new CategoryTreeResponse
                {
                    Id = item?.Id,
                    Name = item?.Name,
                    Children = children,
                }).Children;
        }
    }
}
