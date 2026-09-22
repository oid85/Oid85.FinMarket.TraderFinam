using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Core;
using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;
using Oid85.FinMarket.TraderFinam.WebHost.Controller.Base;

namespace Oid85.FinMarket.TraderFinam.WebHost.Controller;

/// <summary>
/// Трейдер Финам
/// </summary>
[Route("api/trader-finam")]
[ApiController]
public class BrokerController(
    ITraderService brokerService)
    : BaseController
{
    /// <summary>
    /// Получить данные о портфеле
    /// </summary>
    [HttpPost("portfolio")]
    [ProducesResponseType(typeof(BaseResponse<PortfolioInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<PortfolioInfoResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<PortfolioInfoResponse>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetPortfolioAsync(
        [FromBody] PortfolioInfoRequest request) =>
        GetResponseAsync(
            () => brokerService.GetPortfolioInfoAsync(request),
            result => new BaseResponse<PortfolioInfoResponse> { Result = result });
}