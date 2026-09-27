using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Core.Models;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Database.Repositories
{
    public class OutboxRepository(
        IDbContextFactory<TraderFinamContext> contextFactory)
        : IOutboxRepository
    {
        public async Task<List<OutboxTask>> GetAsync()
        {
            await using var context = await contextFactory.CreateDbContextAsync();
            
            var entities = await context.OutboxTaskEntities.AsNoTracking().ToListAsync();

            return [.. entities.Select(Map)];
        }

        public async Task<Guid> AddAsync(OutboxTask model)
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            var entity = Map(model);

            await context.AddAsync(entity);
            await context.SaveChangesAsync();

            return entity.Id;
        }

        public async Task UpdateStateAsync(Guid id, string state)
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            await context.OutboxTaskEntities
                .Where(x => x.Id == id)
                .ExecuteUpdateAsync(x => x
                        .SetProperty(entity => entity.State, state));

            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            await using var context = await contextFactory.CreateDbContextAsync();

            await context.OutboxTaskEntities
                .Where(x => x.Id == id)
                .ExecuteDeleteAsync();
        }

        private static OutboxTask Map(OutboxTaskEntity entity) => 
            new()
            {
                Id = entity.Id,
                Source = entity.Source,
                Type = entity.Type,
                Ticker = entity.Ticker,
                TargetSize = entity.TargetSize,
                TargetStopPrice = entity.TargetStopPrice,
                State = entity.State
            };

        private static OutboxTaskEntity Map(OutboxTask model) =>
            new()
            {
                Source = model.Source,
                Type = model.Type,
                Ticker = model.Ticker,
                TargetSize = model.TargetSize,
                TargetStopPrice = model.TargetStopPrice,
                State = model.State
            };
    }
}
