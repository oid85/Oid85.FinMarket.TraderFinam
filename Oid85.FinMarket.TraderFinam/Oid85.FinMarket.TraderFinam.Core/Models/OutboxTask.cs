namespace Oid85.FinMarket.TraderFinam.Core.Models
{
    public class OutboxTask
    {
        public Guid Id { get; set; }
        public string? Source { get; set; } = null;
        public string? Type { get; set; } = null;
        public string? Ticker { get; set; } = null;
        public string? TargetSize { get; set; } = null;
        public string? TargetStopPrice { get; set; } = null;
        public string? State { get; set; } = null;
    }
}
