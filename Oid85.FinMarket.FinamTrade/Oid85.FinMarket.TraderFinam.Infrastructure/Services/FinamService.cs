using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Net.ClientFactory;
using Grpc.Tradeapi.V1.Auth;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Common.KnownConstants;
using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Services
{
    public class FinamService(
        GrpcClientFactory grpcClientFactory)
        : IFinamService
    {
        public async Task<JwtTokenResponse> GetJwtTokenAsync(JwtTokenRequest request)
        {
            var client = grpcClientFactory.CreateClient<AuthService.AuthServiceClient>(KnownGrpcClients.AuthServiceClient);

            

            return new();
        }
    }
}
