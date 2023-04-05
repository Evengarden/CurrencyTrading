using CurrencyTrading.DAL.DTO;
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
            int userId = int.Parse(User.Claims.FirstOrDefault(x =>
            {
                return x.Type == "ID";
            }).Value);
            var userBalances = await _balanceService.CheckBalance(userId);
            return Ok(userBalances);
        }

        [HttpPost("createBalance")]
        [Authorize]
        public async Task<IActionResult> CreateUserBalance([FromBody] BalanceDTO balance)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(x =>
            {
                return x.Type == "ID";
            }).Value);
            var createdBalance = await _balanceService.AddBalance(userId,balance.Currency,balance.Amount);
            return Ok(createdBalance);
        }
    }
}
