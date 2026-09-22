namespace Oid85.FinMarket.TraderFinam.Core.Responses
{
    public class PortfolioInfoResponse
    {
        public decimal TotalSum { get; set; }
        public List<PositionData> Positions { get; set; } = [];
    }

    public class PositionData
    {
        public string Ticker { get; set; } = string.Empty;
        public int Size { get; set; }
        public decimal Cost { get; set; }
    }
}
