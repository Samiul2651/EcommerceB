using Contracts.Constants;
using Contracts.Interfaces;
using Contracts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private readonly ICategoryService _categoryService;
        public AdminController(ICategoryService categoryService)
        {
           _categoryService = categoryService;
        }

        [HttpPost("addCategory")]
        public async Task<IActionResult> AddCategory(Category category)
        {
            Category newCategory = new Category
            {
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };
            string result = await _categoryService.AddCategory(newCategory);
            switch (result)
            {
                case UpdateStatus.BadRequest:
                    return NotFound(new { Message = "Category Name Already Exists" });
                case UpdateStatus.Success:
                    return Ok(new { Message = "Category Created Successfully" });
                default:
                    return StatusCode(500, "Internal Server Error.");
            }
        }

        [HttpDelete("deleteCategory")]
        public async Task<IActionResult> DeleteCategory(Category category)
        {
            try
            {
                var result = await _categoryService.DeleteCategory(category);
                switch (result)
                {
                    case UpdateStatus.Success:
                        return Ok(new { Message = "Category Created Successfully" });
                    default:
                        return NotFound(new { Message = "Category Name Already Exists" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpPut("updateCategory")]
        public async Task<IActionResult> UpdateCategory(Category category)
        {
            try
            {
                var result = await _categoryService.UpdateCategory(category);
                switch (result)
                {
                    case UpdateStatus.Success:
                        return Ok(new { Message = "Category Updated Successfully" });
                    case UpdateStatus.BadRequest:
                        return BadRequest(new { Message = "Category Name Already Exists" });
                    case UpdateStatus.NotFound:
                        return NotFound(new { Message = "Category Not Found" });
                    default:
                        return StatusCode(500, new { Message = "Internal Server Error." });
                }
            }
            catch(Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }

        }

    }
}
