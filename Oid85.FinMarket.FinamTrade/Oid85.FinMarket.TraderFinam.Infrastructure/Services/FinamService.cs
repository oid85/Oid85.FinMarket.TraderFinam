using Grpc.Net.ClientFactory;
using Grpc.Tradeapi.V1.Auth;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Repositories;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Common.KnownConstants;
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

            var response = new PortfolioInfoResponse();

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

            await tokenRepository.SaveTokenAsync(token, DateTime.Now.AddMinutes(15));

            return token;
        }
    }
}
