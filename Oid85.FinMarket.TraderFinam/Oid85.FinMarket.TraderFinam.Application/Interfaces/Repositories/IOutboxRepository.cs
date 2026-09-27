using Oid85.FinMarket.TraderFinam.Core.Models;

namespace Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories
{
    public interface IOutboxRepository
    {
        Task<List<OutboxTask>> GetAsync();
        Task<Guid> AddAsync(OutboxTask model);
        Task UpdateStateAsync(Guid id, string state);
        Task DeleteAsync(Guid id);
    }
}
