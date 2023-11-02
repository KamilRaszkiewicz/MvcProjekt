using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto.Category;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Helpers;
using MvcProject.Infrastructure.Database;
using System.Linq;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController: ApiController
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ApplicationDbContext context, ILogger<CategoryController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("getTree")]
        public async Task<IActionResult> GetTree()
        {
            var categories = _context.Category
                .Include(x => x.ParentCategory)
                .ToTree(x => x.ParentCategory)
                .ToPoco<CategoryTreeDto>((item, children) => new CategoryTreeDto
                {
                    Id = item == null ? null : item.Id,
                    Name = item == null ? null : item.Name,
                    Children = children,
                }).Children;



            return Ok(categories);
        }

    }
}
