namespace Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        Task<string?> GetTokenAsync(string accountName);
        Task SaveTokenAsync(string accountName, string token, DateTime expire);
    }
}
