using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Common.KnownConstants;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Repositories;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {    
        services.AddDbContextPool<TraderFinamContext>((serviceProvider, options) =>
        {  
            options.UseNpgsql(configuration.GetValue<string>(KnownSettingsKeys.PostgresFinMarketTraderFinamConnectionString)!);
        });

        services.AddPooledDbContextFactory<TraderFinamContext>(options =>
            options
                .UseNpgsql(configuration.GetValue<string>(KnownSettingsKeys.PostgresFinMarketTraderFinamConnectionString)!)
                .EnableServiceProviderCaching(false), poolSize: 32);

        services.AddTransient<IParameterRepository, ParameterRepository>();
    }

    public static async Task ApplyMigrations(this IHost host)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TraderFinamContext>();
        await context.Database.MigrateAsync();
    }
}