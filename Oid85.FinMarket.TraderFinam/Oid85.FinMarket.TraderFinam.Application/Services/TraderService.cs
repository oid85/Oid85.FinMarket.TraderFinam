using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;

namespace Oid85.FinMarket.TraderFinam.Application.Services
{
    /// <inheritdoc />
    public class TraderService(
        IOutboxRepository outboxRepository,
        IFinamService finamService)
        : ITraderService
    {
        /// <inheritdoc />
        public Task<PortfolioInfoResponse> GetPortfolioInfoAsync(PortfolioInfoRequest request) => 
            finamService.GetPortfolioInfoAsync(request);

        /// <inheritdoc />
        public async Task<OutboxTaskListResponse> GetOutboxTaskListAsync(OutboxTaskListRequest request) =>
            new() { Tasks = await outboxRepository.GetAsync() };
    }
}
