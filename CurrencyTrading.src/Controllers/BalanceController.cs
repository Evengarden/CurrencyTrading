using CurrencyTrading.DAL.DTO;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;
        private readonly IAuthService _authService;
        public BalanceController(IBalanceService balanceService, IAuthService authService)
        {
            _balanceService = balanceService;
            _authService = authService;
        }
        [HttpGet("checkBalance")]
        [Authorize]
        public async Task<IActionResult> GetUserBalance()
        {
            int userId = _authService.GetUserId(User.Claims);
            var userBalances = await _balanceService.CheckBalance(userId);
            return Ok(userBalances);
        }

        [HttpPost("createBalance")]
        [Authorize]
        public async Task<IActionResult> CreateUserBalance([FromBody] BalanceDTO balance)
        {
            int userId = _authService.GetUserId(User.Claims);
            var createdBalance = await _balanceService.AddBalance(userId,balance);
            return Ok(createdBalance);
        }
    }
}
