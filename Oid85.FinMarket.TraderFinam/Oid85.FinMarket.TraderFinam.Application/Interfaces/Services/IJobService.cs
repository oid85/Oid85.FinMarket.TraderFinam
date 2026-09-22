namespace Oid85.FinMarket.TraderFinam.Application.Interfaces.Services
{
    /// <summary>
    /// Сервис работы с запланированными задачами
    /// </summary>
    public interface IJobService
    {
        /// <summary>
        /// Загрузить купоны
        /// </summary>
        Task CheckOutboxAsync();
    }
}
