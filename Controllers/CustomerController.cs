using EcommerceWebApi.Constants;
using EcommerceWebApi.DTO;
using EcommerceWebApi.Interfaces;
using EcommerceWebApi.Models;
using EcommerceWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EcommerceWebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ITokenService _tokenService;

        public CustomerController(ICustomerService customerService, ITokenService tokenService)
        {
            _customerService = customerService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public IActionResult LogIn(Customer customer)
        {
            var result = _customerService.LogIn(customer.Email, customer.Password);
            switch (result)
            {
                case UpdateStatus.Success:
                    CustomerDTO customerDto = new CustomerDTO
                    {
                        email = customer.Email,
                        token = _tokenService.GetToken(customer.Email),
                        refreshToken = _tokenService.GetRefreshToken(customer.Email)
                        
                    };
                    return Ok(new
                    {
                        message = "Ok",
                        customer = customerDto
                    });
                case UpdateStatus.NotFound:
                    return NotFound();
                default:
                    return StatusCode(500, "Internal Server Error.");
            }
            
        }

        
        [HttpPost]
        public IActionResult Register(Customer customer)
        {
            var result = _customerService.Register(customer);
            switch (result)
            {
                case UpdateStatus.Success:
                    return Ok(new { message = "Ok" });
                case UpdateStatus.BadRequest:
                    return BadRequest();
                default:
                    return StatusCode(500, "Internal Server Error.");
            }
        }


        [Authorize]
        [HttpPost("order")]
        public IActionResult SubmitOrder(Order order)
        { 
            var result = _customerService.SubmitOrder(order);
            if (result)
            {
                return Ok();
            }
            return StatusCode(500, "Internal Server Error.");
        }

        [HttpPost("token")]
        public IActionResult GetToken(TokenDTO tokenDto)
        {
            //Console.WriteLine(tokenDto.email);
            //Console.WriteLine(tokenDto.token);
            var result = _tokenService.CheckRefreshToken(tokenDto);
            //Console.WriteLine(result);
            if (result)
            {
                var token = _tokenService.GetToken(tokenDto.email);
                return Ok( new
                {
                    token = token
                });
            }
            return BadRequest();
            
        }

        
    }
}
