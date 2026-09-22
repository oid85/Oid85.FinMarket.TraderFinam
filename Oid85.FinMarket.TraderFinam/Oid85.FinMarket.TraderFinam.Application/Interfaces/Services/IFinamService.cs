using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;

namespace Oid85.FinMarket.TraderFinam.Application.Interfaces.Services
{
    /// <summary>
    /// Сервис взаимодействия с API брокера
    /// </summary>
    public interface IFinamService
    {
        /// <summary>
        /// Получить текущее состояние портфеля
        /// </summary>
        Task<PortfolioInfoResponse> GetPortfolioInfoAsync(PortfolioInfoRequest request);

        /// <summary>
        /// Получить актуальный JWT токен
        /// </summary>
        Task<string> GetJwtTokenAsync();
    }
}
