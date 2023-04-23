using CurrencyTrading.DAL.DTO;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class IntegrationController : ControllerBase
    {
        public readonly IIntegrationService _integrationService;
        public IntegrationController(IIntegrationService integrationService)
        {
            _integrationService = integrationService;
        }

        [Authorize]
        [HttpGet("currency")]
        public async Task<IActionResult> GetCurrency()
        {

            var currency = await _integrationService.GetCurrencyFromRedis();
            return Ok(currency);
        }

        [Authorize]
        [HttpGet("calculate")]
        public async Task<IActionResult> CalculateCurrencyPrice([FromBody] BalanceDTO balanceDTO)
        {
            try
            {
                var calculatedValue = await _integrationService.CalculateLotPrice(balanceDTO.Currency, balanceDTO.Amount);
                return Ok(calculatedValue);
            }
            catch (Exception e)
            {

                return BadRequest(e.Message); ;
            }
            
        }
    }
}
