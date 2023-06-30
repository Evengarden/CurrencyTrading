using CurrencyTrading.DAL.DTO;
using CurrencyTrading.Interfaces;
using CurrencyTrading.Models;
using CurrencyTrading.services.Interfaces;
using Quartz;

namespace CurrencyTrading.services.Services
{
    public class MatchingService : IMatchingService, IJob
    {
        private readonly ILotRepository _lotRepository;
        private readonly ITradeService _tradeService;

        public MatchingService(ITradeService tradeService, 
            ILotRepository lotRepository)
        {
            _tradeService = tradeService;
            _lotRepository = lotRepository;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                await MatchLots();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task MatchLots() 
        {

            var allLots = await _lotRepository.GetLotsAsync();

            var lotsForBuy = allLots.Where(l => l.Type == Types.Buy && l.Status != Statuses.Solded 
            && l.Automatch == Automatch.On).ToList();
            var lotsForSold = allLots.Where(l => l.Type == Types.Sold && l.Status != Statuses.Solded 
            && l.Automatch == Automatch.On).ToList();
            foreach (var lot in lotsForSold)
            {
                var approachLot = lotsForBuy.SingleOrDefault(l => 
                                                    l.Currency == lot.Currency &&
                                                    l.OwnerId != lot.OwnerId &&
                                                    l.CurrencyAmount == lot.CurrencyAmount &&
                                                    (l.Price > lot.Price ?
                                                    lot.Price / l.Price * 100 :
                                                    (l.Price / lot.Price * 100)) > 95
                                                   );
                if (approachLot != null)
                {
                    await _tradeService.CreateTrade(new TradeDTO
                    {
                        LotId = approachLot.Id
                    }, lot.OwnerId);
                    await _tradeService.CreateTrade(new TradeDTO
                    {
                        LotId = lot.Id
                    }, approachLot.OwnerId);
                }
            }
        }
    }
}
