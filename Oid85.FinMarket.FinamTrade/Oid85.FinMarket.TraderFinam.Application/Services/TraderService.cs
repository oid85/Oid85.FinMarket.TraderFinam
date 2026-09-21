using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;

namespace Oid85.FinMarket.TraderFinam.Application.Services
{
    /// <inheritdoc />
    public class TraderService(
        IFinamService finamService)
        : ITraderService
    {
        public async Task<PortfolioInfoResponse> GetPortfolioInfoAsync(PortfolioInfoRequest request)
        {
            var response = await finamService.GetJwtTokenAsync(
                new JwtTokenRequest
                {

                });

            return new();
        }
    }
}
