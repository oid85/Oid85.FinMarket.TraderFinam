using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Common.KnownConstants;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Database.Repositories
{
    public class TokenRepository(
        IDbContextFactory<TraderFinamContext> contextFactory) 
        : ITokenRepository
    {
        public async Task<string?> GetTokenAsync()
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var entities = await context.TokenEntities
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

        public async Task SaveTokenAsync(string token, DateTime expire)
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var entity = new TokenEntity
            {
                Name = KnownParameterNames.ApiToken,
                Value = token,
                Expire = expire
            };

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }
    }
}
