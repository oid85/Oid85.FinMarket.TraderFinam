using Microsoft.EntityFrameworkCore;
using Oid85.FinMarket.TraderFinam.Common.KnownConstants;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Entities;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Schemas;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Database;

public class TraderFinamContext(DbContextOptions<TraderFinamContext> options) : DbContext(options)
{
    public DbSet<ParameterEntity> ParameterEntities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder
            .HasDefaultSchema(KnownDatabaseSchemas.Default)
            .ApplyConfigurationsFromAssembly(
                typeof(TraderFinamContext).Assembly,
                type => type
                    .GetInterface(typeof(ITraderFinamSchema).ToString()) != null)
            .UseIdentityAlwaysColumns();
    }    
}