using Business.Services;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : Controller
    {
        ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await _categoryService.GetAllCategories();
                return Ok(new
                {
                    categories
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("rootCategories")]
        public async Task<IActionResult> GetRootCategories()
        {
            try
            {
                var categories = await _categoryService.GetRootCategories();
                return Ok(new
                {
                    categories
                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }

        }

        [HttpGet("getCategoryByParent/{categoryId}")]
        public async Task<IActionResult> GetCategoryByParent(string categoryId)
        {
            try
            {
                var categories = await _categoryService.GetCategoriesByParent(categoryId);
                return Ok(new
                {
                    categories
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("searchCategory/{input}")]
        public async Task<IActionResult> GetCategoryBySearch(string input)
        {
            try
            {
                var categories = await _categoryService.GetCategoriesBySearch(input);
                return Ok(new
                {
                    categories
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("getCategory/{id}")]
        public async Task<IActionResult> GetCategory(string id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category.Id == id)
                {
                    return Ok(category);
                }

                return BadRequest();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("getTopCategories")]
        public async Task<IActionResult> GetTopCategories()
        {
            var categories = await _categoryService.GetTrendingCategories();
            return Ok( new {
                categories
            });
        }
    }
}
