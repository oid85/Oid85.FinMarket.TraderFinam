using Microsoft.AspNetCore.Mvc;
using Oid85.FinMarket.TraderFinam.Application.Interfaces.Services;
using Oid85.FinMarket.TraderFinam.Core;
using Oid85.FinMarket.TraderFinam.Core.Requests;
using Oid85.FinMarket.TraderFinam.Core.Responses;
using Oid85.FinMarket.TraderFinam.WebHost.Controller.Base;

namespace Oid85.FinMarket.TraderFinam.WebHost.Controller;

/// <summary>
/// Инструменты
/// </summary>
[Route("api/instruments")]
[ApiController]
public class InstrumentsController(
    ITraderService traderService)
    : BaseController
{
    /// <summary>
    /// Получить цены
    /// </summary>
    [HttpPost("price")]
    [ProducesResponseType(typeof(BaseResponse<GetInstrumentPriceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(BaseResponse<GetInstrumentPriceResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(BaseResponse<GetInstrumentPriceResponse>), StatusCodes.Status500InternalServerError)]
    public Task<IActionResult> GetInstrumentPriceAsync(
        [FromBody] GetInstrumentPriceRequest request) =>
        GetResponseAsync(
            () => traderService.GetInstrumentPriceAsync(request),
            result => new BaseResponse<GetInstrumentPriceResponse> { Result = result });
}