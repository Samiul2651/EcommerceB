using Contracts.Constants;
using Contracts.Interfaces;
using Contracts.Models;
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

        
        [HttpGet("getProducts/{page}")]
        public IActionResult GetProductsByPage(int page)
        {
            try
            {
                var productsDto = _productService.GetProductsByPage(page);
                var products = productsDto.products;
                var maxPage = productsDto.maxPage;
                if (products.Any())
                {
                    return Ok(new { Message = "Products Found Successfully", products, maxPage });
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
                var productsDto = _productService.GetProductsBySearchAndPage(input, page);
                var products = productsDto.products;
                var maxPage = productsDto.maxPage;
                if (products.Any())
                {
                    return Ok(new { Message = "Products Found Successfully", products, maxPage });

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

                var productsDto = _productService.GetProductsBySearchAndPageWithId(input, page);
                var products = productsDto.products;
                var maxPage = productsDto.maxPage;
                if (products.Any())
                {
                    return Ok(new { Message = "Products Found Successfully", products, maxPage });
                }

                return NoContent();
            }
            catch (Exception ex)
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
                var productsDto = _productService.GetAllProductsByCategory(categoryId, page);
                var products = productsDto.products;
                var maxPage = productsDto.maxPage;
                return Ok(new
                {
                    products,
                    maxPage
                });
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return StatusCode(500, new { Message = "Internal Server Error." });
            }
        }

        


        [HttpPost("getProductsByIds")]
        public IActionResult GetProductsByIds(List<string> ids)
        {
            try
            {
                var products = _productService.GetProductsByIds(ids);
                return Ok(
                    new
                    {
                        products
                    });
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        [HttpGet("upvoteProduct/{productId}/{userId}")]
        public IActionResult UpvoteProduct(string productId, string userId)
        {
            _productService.UpvoteProduct(productId, userId);
            return Ok();
        }

        [HttpGet("downvoteProduct/{productId}/{userId}")]
        public IActionResult DownvoteProduct(string productId, string userId)
        {
            _productService.DownvoteProduct(productId, userId);
            return Ok();
        }
    }
}