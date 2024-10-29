using EcommerceWebApi.DTO;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using EcommerceWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private ICustomerService _customerService;
        private ITokenService _tokenService;

        public CustomerController(ICustomerService customerService, ITokenService tokenService)
        {
            _customerService = customerService;
            _tokenService=tokenService;
        }

        //[AllowAnonymous]
        [HttpPost("login")]
        //public IActionResult LogIn(Customer customer)
        public IActionResult LogIn(Customer customer)
        {
            //Console.WriteLine("Recieved");
            bool result = _customerService.LogIn(customer.Email, customer.Password);
            //Console.WriteLine(result);
            if (result)
            {
                CustomerDTO customerDto = new CustomerDTO
                {
                    name = customer.Name,
                    token = _tokenService.GetToken(customer)
                };
                return Ok(new
                {
                    message = "Ok",
                    customer = customerDto
                });
            }

            return NotFound();
        }

        
        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            //Console.WriteLine(customer);
            bool result = _customerService.Register(customer);
            if (result)
            {
                return Ok(new { message = "Ok" });
            }
            else return BadRequest();
        }

        [HttpPost("order")]
        public void SubmitOrder(Order order)
        { 
            Console.WriteLine(order);
            Console.WriteLine(order.Price);
            _customerService.SubmitOrder(order);
        }
    }
}
