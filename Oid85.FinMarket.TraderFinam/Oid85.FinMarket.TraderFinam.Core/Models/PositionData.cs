namespace Oid85.FinMarket.TraderFinam.Core.Models
{
    public class PositionData
    {
        public string Ticker { get; set; } = string.Empty;
        public int Size { get; set; }
        public decimal Cost { get; set; }
    }
}
