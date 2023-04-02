using CurrencyTrading.Helper;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly JWTSettings _config;
        public UserController(IUserRepository userRepository, IOptions<JWTSettings> config)
        {
            _userRepository = userRepository;
            _config = config.Value;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> Registration([FromBody] User user)
        {
            try
            {
                user.Password = HashPassword.HashPass(user.Password);
                var createdUser = await _userRepository.CreateUserAsync(user);

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
            var findedUser = await _userRepository.Auth(user.Login,user.Password);
            if (findedUser != null)
            {
                var token = AuthHelper.GenerateJwtToken(findedUser,_config);
                return Ok(token);
            }

            return BadRequest("Invalid credentails");
        }
        [HttpGet("getUser")]
        [Authorize]
        public async Task<IActionResult> GetUserAsync()
        {
          var user = await _userRepository.GetUserAsync(int.Parse(User.Claims.FirstOrDefault(x => x.Type == "ID").Value));

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
