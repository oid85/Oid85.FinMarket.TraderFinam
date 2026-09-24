using Oid85.FinMarket.TraderFinam.Core.Models;

namespace Oid85.FinMarket.TraderFinam.Core.Responses
{
    public class PortfolioInfoResponse
    {
        public decimal TotalSum { get; set; }
        public decimal Money { get; set; }
        public decimal TotalDailyPnl { get; set; }
        public List<PositionData> Positions { get; set; } = [];
    }
}
