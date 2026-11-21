using Grpc.Core;
using Grpc.Net.ClientFactory;
using Grpc.Tradeapi.V1.Accounts;
using Grpc.Tradeapi.V1.Auth;
using Grpc.Tradeapi.V1.Marketdata;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Common.KnownConstants;
using Oid85.FinMarket.TraderFinam.Common.Utils;
using Oid85.FinMarket.TraderFinam.Core.Models;
using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Services
{
    /// <inheritdoc />
    public class FinamService(
        GrpcClientFactory grpcClientFactory,
        IParameterRepository parameterRepository,
        ITokenRepository tokenRepository)
        : IFinamService
    {
        /// <inheritdoc />
        public async Task<GetInstrumentPriceResponse> GetInstrumentPriceAsync(GetInstrumentPriceRequest request)
        {
            string token = await GetJwtTokenAsync(request.AccountName);

            var metadata = new Metadata { { "Authorization", $"Bearer {token}" } };

            var client = grpcClientFactory.CreateClient<MarketDataService.MarketDataServiceClient>(KnownGrpcClients.MarketDataServiceClient);

            var items = new List<TickerPriceItem>();

            foreach (var ticker in request.Tickers)
            {
                var quoteRequest = new QuoteRequest { Symbol = ticker };
                var quoteResponse = await client.LastQuoteAsync(quoteRequest, metadata);
                var price = quoteResponse.Quote.Close.Value.ToDecimal();

                items.Add(new TickerPriceItem { Ticker = ticker, Price = Convert.ToDouble(price) });
            }

            return new () { Items = items };
        }

        /// <inheritdoc />
        public async Task<PortfolioInfoResponse> GetPortfolioInfoAsync(PortfolioInfoRequest request)
        {
            string token = await GetJwtTokenAsync(request.AccountName);

            var metadata = new Metadata { { "Authorization", $"Bearer {token}" } };

            var client = grpcClientFactory.CreateClient<AccountsService.AccountsServiceClient>(KnownGrpcClients.AccountsServiceClient);

            string accountId = await parameterRepository.GetParameterValueAsync($"Account:{request.AccountName}") ?? throw new NullReferenceException();

            var accountRequest = new GetAccountRequest { AccountId = accountId };

            var accountResponse = await client.GetAccountAsync(accountRequest, metadata);

            decimal totalSum = Math.Round(accountResponse.Equity.Value.ToDecimal(), 2);

            decimal totalDailyPnl = 0;
            decimal money = 0;

            foreach (var cashItem in accountResponse.Cash.Where(x => x.CurrencyCode == "RUB"))
                money += cashItem.Units + cashItem.Nanos / 1_000_000_000m;

            var positions = new List<PositionData>();

            foreach (var position in accountResponse.Positions)
            {
                string ticker = position.Symbol.Replace("@MISX", "");
                int size = Convert.ToInt32(position.Quantity.Value.Replace("+", "").ToDouble());
                decimal currentPrice = position.CurrentPrice.Value.ToDecimal();
                decimal dailyPnl = position.DailyPnl.Value.ToDecimal();
                decimal cost = currentPrice * size;

                totalDailyPnl += dailyPnl;

                positions.Add(
                    new PositionData
                    {
                        Ticker = ticker,
                        Size = size,
                        Cost = cost,
                        CurrentPrice = currentPrice,
                        DailyPnl = dailyPnl
                    });
            }

            var response = new PortfolioInfoResponse 
            {
                TotalSum = totalSum,
                TotalDailyPnl = totalDailyPnl,
                Money = money,
                Positions = positions
            };

            return response;
        }

        /// <inheritdoc />
        public async Task<string> GetJwtTokenAsync(string accountName)
        {
            string? token = await tokenRepository.GetTokenAsync(accountName);

            if (token is not null)
                return token;

            var client = grpcClientFactory.CreateClient<AuthService.AuthServiceClient>(KnownGrpcClients.AuthServiceClient);

            string apiToken = await parameterRepository.GetParameterValueAsync($"Token:{accountName}") ?? throw new NullReferenceException();

            var authRequest = new AuthRequest { Secret = apiToken };

            var authResponse = await client.AuthAsync(authRequest);

            token = authResponse.Token;

            await tokenRepository.SaveTokenAsync(accountName, token, DateTime.UtcNow.AddMinutes(15));

            return token;
        }
    }
}
