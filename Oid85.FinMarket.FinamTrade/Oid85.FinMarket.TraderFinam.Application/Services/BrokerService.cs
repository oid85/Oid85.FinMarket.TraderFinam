using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;

namespace Oid85.FinMarket.TraderFinam.Application.Services
{
    /// <inheritdoc />
    public class BrokerService : IBrokerService
    {
        public Task<PortfolioInfoResponse> GetPortfolioInfoAsync(PortfolioInfoRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
