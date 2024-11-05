using Contracts.Interfaces;
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
    }
}
