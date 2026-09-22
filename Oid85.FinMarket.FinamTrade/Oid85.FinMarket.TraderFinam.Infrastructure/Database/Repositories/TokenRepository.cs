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

            var entity = await context.TokenEntities.FirstOrDefaultAsync(x => x.Expire >= DateTime.Now.AddMinutes(5));

            if (entity is null)
            {
                await context.TokenEntities.ExecuteDeleteAsync();
                return null;
            }

            return entity.Value;
        }

        public async Task SaveTokenAsync(string token, DateTime expire)
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var entity = new TokenEntity
            {
                Id = Guid.NewGuid(),
                Name = KnownParameterNames.ApiToken,
                Value = token,
                Expire = expire
            };

            await context.AddAsync(entity);
            await context.SaveChangesAsync();
        }
    }
}
