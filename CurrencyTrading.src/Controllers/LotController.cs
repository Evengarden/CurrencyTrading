using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Helper;
using CurrencyTrading.Models;
using CurrencyTrading.services.CustomExceptions;
using CurrencyTrading.services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyTrading.Controllers
{
    [ApiController]
    public class LotController : ControllerBase
    {
        private readonly ILotService _lotService;
        private readonly IIntegrationService _integrationService;
        public LotController(ILotService lotService, IIntegrationService integrationService)
        {
            _lotService = lotService;
            _integrationService = integrationService;
        }

        [Authorize]
        [HttpGet("lot/{id:int}")]
        public async Task<IActionResult> GetLot(int id)
        {
            var lot = await _lotService.GetLot(id);
            return Ok(lot);
        }

        [Authorize]
        [HttpGet("getLots")]
        public async Task<IActionResult> GetLots()
        {
            int userId = GetCurrentUserId.GetUserId(User.Claims);
            var lots = await _lotService.GetLots(userId);
            return Ok(lots);
        }

        [Authorize]
        [HttpPatch("updateLot/{id:int}")]
        public async Task<IActionResult> UpdateLot(int id,[FromBody] LotDTO lot)
        {
            try
            {
                await _integrationService.CheckCurrencyExist(lot.Currency);
                int userId = GetCurrentUserId.GetUserId(User.Claims);
                var updatedLot = await _lotService.UpdateLot(id, lot, userId);
                return Ok(updatedLot);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpDelete("deleteLot/{id:int}")]
        public async Task<IActionResult> DeleteLot(int id)
        {
            try
            {
                var deletedLot = await _lotService.DeleteLot(id);
                return Ok(deletedLot);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpPost("createLot")]
        public async Task<IActionResult> CreateLot([FromBody] LotDTO lot)
        {
            try
            {
                await _integrationService.CheckCurrencyExist(lot.Currency);
                int userId = GetCurrentUserId.GetUserId(User.Claims);
                var createdLot = await _lotService.CreateLot(userId, lot);
                return Ok(createdLot);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            
        }
    }
}
