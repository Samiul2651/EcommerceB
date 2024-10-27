using EcommerceWebApi.Models;
using EcommerceWebApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace EcommerceWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("login")]
        public IActionResult LogIn(Customer customer)
        {
            Console.WriteLine("Recieved");
            bool result = _customerService.LogIn(customer.Email, customer.Password);
            Console.WriteLine(result);
            if (result)
            {
                return Ok(new { message = "Ok"});
            }
            return NotFound();
            //return _customerService.LogIn(email, password);
        }

        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            Console.WriteLine(customer);
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
