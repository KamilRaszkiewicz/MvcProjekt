using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MvcProject.Application.Dto.Category;
using MvcProject.Application.Dto.User;
using MvcProject.Application.Helpers;
using MvcProject.Application.Interfaces;
using MvcProject.Infrastructure.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MvcProject.API.Controllers
{
    [ApiController]
    [Route("api/category")]
    public class CategoryController: ApiController
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ICategoryService categoryService, ILogger<CategoryController> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        [AllowAnonymous]
        [HttpPost("getTree")]
        public async Task<ActionResult<ICollection<CategoryTreeDto>>> GetTree()
        {
            ICollection<CategoryTreeDto> tree;

            try
            {
                tree = _categoryService.GetCategoryTree();
            }
            catch (Exception e)
            {
                _logger.LogError("Exception caught in {0}: {1}", nameof(GetTree), e);

                return StatusCode(500);
            }

            return Ok(tree);
        }
    }
}
