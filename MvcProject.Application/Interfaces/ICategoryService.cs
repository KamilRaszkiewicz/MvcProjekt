using MvcProject.Application.Dto.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvcProject.Application.Interfaces
{
    public interface ICategoryService
    {
        ICollection<CategoryTreeResponse> GetCategoryTree();
    }
}
