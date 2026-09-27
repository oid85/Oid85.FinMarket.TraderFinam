using Oid85.FinMarket.TraderFinam.Core.Models;

namespace Oid85.FinMarket.TraderFinam.Core.Responses
{
    public class OutboxTaskListResponse
    {
        public List<OutboxTask> Tasks { get; set; } = [];
    }
}
