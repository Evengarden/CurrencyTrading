using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Helper;
using CurrencyTrading.Models;
using CurrencyTrading.services.Interfaces;
using CurrencyTrading.services.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class TradeController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        public TradeController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        [Authorize]
        [HttpGet("trade/{id:int}")]
        public async Task<IActionResult> GetTrade(int id)
        {
            var trade = await _tradeService.GetTrade(id);
            return Ok(trade);
        }

        [Authorize]
        [HttpGet("trades")]
        public async Task<IActionResult> GetTrades()
        {
            var trades = await _tradeService.GetTrades();
            return Ok(trades);
        }

        [Authorize]
        [HttpPost("createTrade")]
        public async Task<IActionResult> CreateTrade([FromBody] TradeDTO tradeDTO)
        {
            try
            {
                int userId = GetCurrentUserId.GetUserId(User.Claims);
                var createdTrade = await _tradeService.CreateTrade(tradeDTO, userId);
                return Ok(createdTrade);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
