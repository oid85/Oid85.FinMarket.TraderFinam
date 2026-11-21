using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Database.Repositories
{
    public class TokenRepository(
        IDbContextFactory<TraderFinamContext> contextFactory) 
        : ITokenRepository
    {
        public async Task<string?> GetTokenAsync(string accountName)
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var entities = await context.TokenEntities
                .Where(x => x.Name == $"Token:{accountName}")
                .AsNoTracking()
                .ToListAsync();

            var notExpired = entities.Where(x => x.Expire >= DateTime.UtcNow.AddMinutes(5)).ToList();

            if (notExpired.Count == 0)
            {
                await context.TokenEntities.ExecuteDeleteAsync();
                return null;
            }

            return entities[0].Value;
        }

        public async Task SaveTokenAsync(string accountName, string token, DateTime expire)
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var entity = new TokenEntity
            {
                Name = $"Token:{accountName}",
                Value = token,
                Expire = expire
            };

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }
    }
}
