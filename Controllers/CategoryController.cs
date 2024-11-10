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
        public IActionResult GetAllCategories()
        {
            try
            {
                var categories = _categoryService.GetAllCategories();
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
        public IActionResult GetRootCategories()
        {
            try
            {
                var categories = _categoryService.GetRootCategories();
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
        public IActionResult GetCategoryByParent(string categoryId)
        {
            try
            {
                var categories = _categoryService.GetCategoriesByParent(categoryId);
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
        public IActionResult GetCategoryBySearch(string input)
        {
            try
            {
                var categories = _categoryService.GetCategoriesBySearch(input);
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
        public IActionResult GetCategory(string id)
        {
            try
            {
                var category = _categoryService.GetCategoryById(id);
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
    }
}
