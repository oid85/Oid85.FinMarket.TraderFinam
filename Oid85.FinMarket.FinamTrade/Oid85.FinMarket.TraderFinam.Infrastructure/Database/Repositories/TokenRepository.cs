using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Database.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        public Task<string?> GetTokenAsync()
        {
            throw new NotImplementedException();
        }

        public Task SaveTokenAsync(string token, DateTime expire)
        {
            throw new NotImplementedException();
        }
    }
}
