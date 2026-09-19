using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;

namespace Oid85.FinMarket.TraderFinam.Application.Interfaces.Services
{
    public interface IFinamService
    {
        Task<JwtTokenResponse> GetJwtTokenAsync(JwtTokenRequest request);
    }
}
