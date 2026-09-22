using Grpc.Core;
using Grpc.Net.ClientFactory;
using Grpc.Tradeapi.V1.Accounts;
using Grpc.Tradeapi.V1.Auth;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Common.KnownConstants;
using Oid85.FinMarket.TraderFinam.Common.Utils;
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
        public async Task<PortfolioInfoResponse> GetPortfolioInfoAsync(PortfolioInfoRequest request)
        {
            string token = await GetJwtTokenAsync();

            var metadata = new Metadata { { "Authorization", $"Bearer {token}" } };

            var client = grpcClientFactory.CreateClient<AccountsService.AccountsServiceClient>(KnownGrpcClients.AccountsServiceClient);

            string accountId = await parameterRepository.GetParameterValueAsync(KnownParameterNames.AccountId) ?? throw new NullReferenceException();

            var accountRequest = new GetAccountRequest { AccountId = accountId };

            var accountResponse = await client.GetAccountAsync(accountRequest, metadata);

            var totalSum = accountResponse.Equity.Value.ToDecimal();

            var positions = new List<PositionData>();

            foreach (var position in accountResponse.Positions)
            {
                string ticker = position.Symbol.Replace("@MISX", "");
                int size = Convert.ToInt32(position.Quantity.Value.Replace("+", "").ToDouble());
                decimal price = position.CurrentPrice.Value.ToDecimal();
                decimal cost = price * size;

                positions.Add(
                    new PositionData
                    {
                        Ticker = ticker,
                        Size = size,
                        Cost = cost
                    });
            }

            var response = new PortfolioInfoResponse 
            {
                TotalSum = totalSum ,
                Positions = positions
            };

            return response;
        }

        /// <inheritdoc />
        public async Task<string> GetJwtTokenAsync()
        {
            string? token = await tokenRepository.GetTokenAsync();

            if (token is not null)
                return token;

            var client = grpcClientFactory.CreateClient<AuthService.AuthServiceClient>(KnownGrpcClients.AuthServiceClient);

            string apiToken = await parameterRepository.GetParameterValueAsync(KnownParameterNames.ApiToken) ?? throw new NullReferenceException();

            var authRequest = new AuthRequest { Secret = apiToken };

            var authResponse = await client.AuthAsync(authRequest);

            token = authResponse.Token;

            await tokenRepository.SaveTokenAsync(token, DateTime.UtcNow.AddMinutes(15));

            return token;
        }
    }
}
