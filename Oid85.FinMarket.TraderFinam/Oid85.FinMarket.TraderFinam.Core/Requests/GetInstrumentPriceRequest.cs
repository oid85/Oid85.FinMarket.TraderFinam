namespace Oid85.FinMarket.TraderFinam.Core.Requests
{
    public class GetInstrumentPriceRequest
    {
        public string AccountName { get; set; } = string.Empty;
        public List<string> Tickers { get; set; } = [];
    }
}
