using CurrencyTradig.CbClient.Interfaces;
using CurrencyTrading.DAL.DTO;
using RestSharp;
using System.Xml.Linq;

namespace CurrencyTradig.CbClient
{
    public class CbClient : ICbApiClient
    {
        private readonly ICustomMapper _customMapper;
        public CbClient(ICustomMapper customMapper)
        {
            _customMapper = customMapper;

        }
        public async Task<ICollection<CurrencyDTO>> sendRequestToCb()
        {
            var client = new RestClient("http://www.cbr.ru/scripts/XML_daily.asp");

            var request = new RestRequest();

            var response = await client.GetAsync(request);
            var xElement = XElement.Parse(response.Content);
            var currencies = _customMapper.XmlToDto(xElement);

            return currencies;
        }
    }
}