using EcommerceWebApi.Constants;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using EcommerceWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller {
        
        private readonly IProductService _productService;
        private readonly IMongoDbService _mongoDbService;
        public ProductController(IProductService productService, IMongoDbService mongoDbService)
        {
            _productService = productService;
            _mongoDbService = mongoDbService;
        }


        [HttpPost]
        public IActionResult AddProduct(Product newProduct)
        {
            //Console.WriteLine(newProduct.Price);
            bool productAddResult = _productService.AddProduct(newProduct);
            var uri = Url.Action("GetProduct", new { id = newProduct.Id });
            if (productAddResult == true)
            {
                return Created(uri, newProduct);
            }
            else
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }

        }


        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {
            if (!_mongoDbService.IsAlive())
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
            else
            {
                Console.WriteLine(product.Price);
                var productUpdateResult = _productService.UpdateProduct(product);
                switch (productUpdateResult)
                {
                    case UpdateStatus.NotFound:
                        return NotFound(new { Message = "No Product Found" });
                    case UpdateStatus.Success:
                        return Ok(new { Message = "Product Updated Successfully" });
                    default:
                        return StatusCode(500, "Internal Server Error.");
                }

            }
        }

        [HttpGet("{id}")]
        public IActionResult GetProduct(string id)
        {
            if (!_mongoDbService.IsAlive())
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
            var product = _productService.GetProductById(id);
            if (product == null || product.Id != id)
            {
                return NotFound(new { Message = "No Product Found." });
            }
            else
            {
                return Ok(new { Message = "Product Found Successfully", Product = product });
            }
        }

        [HttpGet("getProducts/{page}")]
        public IActionResult GetProductsByPage(int page)
        {
            if (!_mongoDbService.IsAlive())
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
            var products = _productService.GetProductsByPage(page);
            if (products.Any())
            {
                return Ok(new { Message = "Products Found Successfully", products });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("getProductsBySearch/{input}/{page}")]
        public IActionResult GetProductsBySearch(string input, int page)
        {
            if (!_mongoDbService.IsAlive())
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
            var products = _productService.GetProductsBySearchAndPage(input, page);
            if (products.Any())
            {
                return Ok(new { Message = "Products Found Successfully", products });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpGet("getProductsBySearchWithId/{input}/{page}")]
        public IActionResult GetProductsBySearchWithId(string input, int page)
        {
            if (!_mongoDbService.IsAlive())
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
            var products = _productService.GetProductsBySearchAndPageWithId(input, page);
            if (products.Any())
            {
                return Ok(new { Message = "Products Found Successfully", products });
            }
            else
            {
                return NoContent();
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
        {
            if (!_mongoDbService.IsAlive())
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
            else
            {
                var productDeleteResult = _productService.DeleteProduct(id);

                switch (productDeleteResult)
                {
                    case UpdateStatus.NotFound:
                        return NotFound(new { Message = "Product Not Found." });
                    case UpdateStatus.Success:
                        return Ok(new { Message = "Product Deleted Successfully." });
                    default:
                        return StatusCode(500, "Internal Server Error.");
                }


            }
        }

        [HttpGet("categories")]
        public List<Category> GetAllCategories()
        {
            return _productService.GetAllCategories();
        }

        [HttpGet("productsByCategory/{categoryId}/{page}")]
        public List<Product> GetProductsByCategory(string categoryId, int page)
        {
            var products = _productService.GetAllProductsByCategory(categoryId, page);
            return products;
        }

        [HttpGet("rootCategories")]
        public List<Category> GetRootCategories()
        {
            var categories = _productService.GetRootCategories();
            return categories;
        }

        [HttpGet("getCategoryByParent/{categoryId}")]
        public List<Category> GetCategoryByParent(string categoryId)
        {
            var categories = _productService.GetCategoriesByParent(categoryId);
            return categories;
        }
    }
}