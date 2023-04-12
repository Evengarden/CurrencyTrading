using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System.Data;
using System.Text;

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
        [HttpGet("currency")]
        public async Task<IActionResult> GetCurrency()
        {

            var currency = await _integrationService.GetCurrencyFromRedis();
            return Ok(currency);
        }
    }
}
