using MvcProject.Application.Dto.Category;
using MvcProject.Application.Helpers;
using MvcProject.Application.Interfaces;
using MvcProject.Domain.Models;
using System.Collections.Generic;
using System.Linq;

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

        public ICollection<int> GetDescendantCategoryIds(int categoryId)
        {
            var category = _repository
                .GetAll(x => x.ParentCategory)
                .ToTree(x => x.ParentCategory)
                .First(x => x.Id == categoryId);

            if(category == null)
            {
                return new List<int>();
            }

            return category.Flatten(x => x.Id).ToList();
        }
    }
}
