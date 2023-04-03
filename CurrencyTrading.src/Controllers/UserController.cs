using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.Helpers;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] User user)
        {
            try
            {
               await _userService.UserRegistration(user);
               return Ok("User succesfully created");
            }
            catch (DbUpdateException)
            {
                return BadRequest("Login already exist");
            }
        }

        [HttpPost("auth")]
        public async Task<IActionResult> Authorization([FromBody] User user)
        {
            var token = await _userService.Auth(user);
            if (token != null)
            {
                return Ok(token);
            }

            return BadRequest("Invalid credentails");
        }
        [HttpGet("getUser")]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(x =>
            {
                return x.Type == "ID";
            }).Value);
            var user = await _userService.GetCurrentUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
