using System;
using System.Collections.Generic;
using System.Text;
using Grpc.Tradeapi.V1.Auth;

namespace Oid85.FinMarket.TraderFinam.Infrastructure.Services
{
    public class FinamService(AuthService.AuthServiceClient authService)
    {
    }
}
