using CurrencyTrading.DAL.DTO;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        public readonly ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService integrationService)
        {
            _currencyService = integrationService;
        }

        [Authorize]
        [HttpGet("currency")]
        public async Task<IActionResult> GetCurrency()
        {

            var currency = await _currencyService.GetCurrency();
            return Ok(currency);
        }

        [Authorize]
        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateCurrencyPrice([FromBody] BalanceDTO balanceDTO)
        {
            try
            {
                var calculatedValue = await _currencyService.CalculateLotPrice(balanceDTO.Currency, balanceDTO.Amount);
                return Ok(calculatedValue);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message);
            }
            
        }
    }
}
