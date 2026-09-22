using Grpc.Tradeapi.V1.Accounts;
using Grpc.Tradeapi.V1.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Common.KnownConstants;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database;
using Oid85.FinMarket.TraderFinam.Infrastructure.Database.Repositories;
using Oid85.FinMarket.TraderFinam.Infrastructure.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Infrastructure.Services;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureDatabase(
        this IServiceCollection services,
        IConfiguration configuration)
    {    
        services.AddDbContextPool<TraderFinamContext>((serviceProvider, options) =>
        {  
            options.UseNpgsql(configuration.GetValue<string>(KnownSettingsKeys.PostgresFinMarketTraderFinamConnectionString));
        });

        services.AddPooledDbContextFactory<TraderFinamContext>(options =>
            options
                .UseNpgsql(configuration.GetValue<string>(KnownSettingsKeys.PostgresFinMarketTraderFinamConnectionString))
                .EnableServiceProviderCaching(false), poolSize: 32);

        services.AddTransient<IParameterRepository, ParameterRepository>();
        services.AddTransient<ITokenRepository, TokenRepository>();
    }

    public static void ConfigureFinamGrpcClient(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddGrpcClient<AuthService.AuthServiceClient>(KnownGrpcClients.AuthServiceClient, options =>
        {
            options.Address = new Uri(configuration.GetValue<string>(KnownSettingsKeys.FinamGrpcUrl)!);
        });

        services.AddGrpcClient<AccountsService.AccountsServiceClient>(KnownGrpcClients.AccountsServiceClient, options =>
        {
            options.Address = new Uri(configuration.GetValue<string>(KnownSettingsKeys.FinamGrpcUrl)!);
        });
    }

    public static void ConfigureInfrastructureServices(
    this IServiceCollection services)
    {
        services.AddTransient<IFinamService, FinamService>();
        services.AddTransient<ITokenService, TokenService>();
    }

    public static async Task ApplyMigrations(this IHost host)
    {
        var scopeFactory = host.Services.GetRequiredService<IServiceScopeFactory>();
        await using var scope = scopeFactory.CreateAsyncScope();
        await using var context = scope.ServiceProvider.GetRequiredService<TraderFinamContext>();
        await context.Database.MigrateAsync();
    }
}