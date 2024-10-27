using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using EcommerceWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : Controller
    {
        private IMongoDbService _mongoDbService;
        public AdminController(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        [HttpPut("addCategoryToProduct")]
        public void AddCategoryToProduct(string categoryId, string productId)
        {
            _mongoDbService.AddCategoryToProduct(categoryId, productId);
        }

        [HttpPost("addCategory")]
        public void AddCategory(Category category)
        {
            Category newCategory = new Category
            {
                Name = category.Name,
                ParentCategoryId = category.ParentCategoryId
            };
            _mongoDbService.AddObject(nameof(Category), newCategory);
        }

    }
}
