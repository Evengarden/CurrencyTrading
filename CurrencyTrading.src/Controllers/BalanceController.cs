using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Helper;
using CurrencyTrading.Models;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class BalanceController : ControllerBase
    {
        private readonly IBalanceService _balanceService;
        public BalanceController(IBalanceService balanceService)
        {
            _balanceService = balanceService;
        }
        [HttpGet("checkBalance")]
        [Authorize]
        public async Task<IActionResult> GetUserBalance()
        {
            int userId = GetCurrentUserId.GetUserId(User.Claims);
            var userBalances = await _balanceService.CheckBalance(userId);
            return Ok(userBalances);
        }

        [HttpPost("createBalance")]
        [Authorize]
        public async Task<IActionResult> CreateUserBalance([FromBody] BalanceDTO balance)
        {
            int userId = GetCurrentUserId.GetUserId(User.Claims);
            var createdBalance = await _balanceService.AddBalance(userId,balance);
            return Ok(createdBalance);
        }
    }
}
