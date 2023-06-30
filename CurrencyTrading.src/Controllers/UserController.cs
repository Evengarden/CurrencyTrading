using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Models;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        public UserController(IUserService userService, IAuthService authService)
        {
            _userService = userService;
            _authService = authService;
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
        public async Task<IActionResult> Authorization([FromBody] UserDTO user)
        {
            try
            {
                var token = await _userService.Auth(user);
                if (token != null)
                {
                    return Ok(token);
                }

                return BadRequest("Invalid credentails");
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
        [HttpGet("getUser")]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
            int userId = _authService.GetUserId(User.Claims);
            var user = await _userService.GetCurrentUser(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [Authorize]
        [HttpPatch("updateUser")]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserDTO user)
        {
            try
            {
                int userId = _authService.GetUserId(User.Claims);
                var updatedUser = await _userService.UpdateUser(userId, user);
                return Ok(updatedUser);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }

        }
    }
}
