using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities.Base;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities
{
    public class OutboxTaskEntity : BaseEntity
    {
        public string? Source { get; set; } = null;
        public string? Type { get; set; } = null;
        public string? Ticker { get; set; } = null;
        public string? TargetSize { get; set; } = null;
        public string? TargetStopPrice { get; set; } = null;
        public string? State { get; set; } = null;
    }
}
