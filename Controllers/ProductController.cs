using System.Diagnostics.CodeAnalysis;
using EcommerceWebApi.Constants;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using EcommerceWebApi.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpPost]
        public IActionResult AddProduct(Product newProduct)
        {
            //Console.WriteLine(newProduct.Price);
            bool productAddResult = _productService.AddProduct(newProduct);
            var uri = Url.Action("GetProduct", new { id = newProduct.Id });
            if (productAddResult == true)
            {
                return Created(uri, newProduct);
                //return Ok();
            }
            else
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }

        }

        [Authorize]
        [HttpPut]
        public IActionResult UpdateProduct(Product product)
        {
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

        
        [HttpGet("{id}")]
        public IActionResult GetProduct(string id)
        {
            try
            {
                var product = _productService.GetProductById(id);
                if (product == null || product.Id != id)
                {
                    return NotFound(new { Message = "No Product Found." });
                }

                return Ok(new { Message = "Product Found Successfully", Product = product });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }

        }

        [Authorize]
        [HttpGet("getProducts/{page}")]
        public IActionResult GetProductsByPage(int page)
        {
            try
            {
                var products = _productService.GetProductsByPage(page);
                if (products.Any())
                {
                    return Ok(new { Message = "Products Found Successfully", products });
                }

                return NoContent();
            }
            catch
            {
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("getProductsBySearch/{input}/{page}")]
        public IActionResult GetProductsBySearch(string input, int page)
        {
            try
            {
                var products = _productService.GetProductsBySearchAndPage(input, page);
                if (products.Any())
                {
                    return Ok(new { Message = "Products Found Successfully", products });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("getProductsBySearchWithId/{input}/{page}")]
        public IActionResult GetProductsBySearchWithId(string input, int page)
        {
            try
            {
                var products = _productService.GetProductsBySearchAndPageWithId(input, page);
                if (products.Any())
                {
                    return Ok(new { Message = "Products Found Successfully", products });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(string id)
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

        [HttpGet("categories")]
        public IActionResult GetAllCategories()
        {
            try
            {
                var categories = _productService.GetAllCategories();
                return Ok( new {
                    categories = categories
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        [HttpGet("productsByCategory/{categoryId}/{page}")]
        public IActionResult GetProductsByCategory(string categoryId, int page)
        {
            try
            {
                var products = _productService.GetAllProductsByCategory(categoryId, page);
                return Ok(new
                {
                    products = products
                });
            }
            catch(Exception ex)
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
                var categories = _productService.GetRootCategories();
                return Ok(new
                {
                    categories = categories
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
                var categories = _productService.GetCategoriesByParent(categoryId);
                return Ok(new
                {
                    categories = categories
                });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
            
            
        }
    }
}