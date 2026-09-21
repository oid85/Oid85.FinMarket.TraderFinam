namespace Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories
{
    public interface ITokenRepository
    {
        Task<string?> GetTokenAsync();
        Task SaveTokenAsync(string token, DateTime expire);
    }
}
