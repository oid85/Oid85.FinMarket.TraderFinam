using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;

namespace Oid85.FinMarket.TraderFinam.Application.Interfaces.Services
{
    /// <summary>
    /// Брокер сервис
    /// </summary>
    public interface IBrokerService
    {
        Task<PortfolioInfoResponse> GetPortfolioInfoAsync(PortfolioInfoRequest request);
    }
}
