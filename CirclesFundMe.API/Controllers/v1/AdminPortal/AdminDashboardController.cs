namespace CirclesFundMe.API.Controllers.v1.AdminPortal
{
    public class AdminDashboardController(ISender sender) : BaseControllerV1
    {
        private readonly ISender _sender = sender;

        [HttpGet("statistics")]
        [ProducesResponseType<BaseResponse<DashboardStatisticsModel>>(200)]
        [SwaggerOperation(Summary = "Get Dashboard Statistics for Admin Portal")]
        public async Task<IActionResult> GetDashboardStatistics(CancellationToken cancellation)
        {
            BaseResponse<DashboardStatisticsModel> response = await _sender.Send(new GetDashboardStatisticsQuery(), cancellation);
            return HandleResponse(response);
        }

        [HttpGet("inflows")]
        [ProducesResponseType<BaseResponse<PagedList<AdminTransactionModel>>>(200)]
        [SwaggerOperation(Summary = "Get Admin Inflow Transactions")]
        public async Task<IActionResult> GetAdminInflowTransactions([FromQuery] MinimalParams @params, CancellationToken cancellation)
        {
            BaseResponse<PagedList<AdminTransactionModel>> response = await _sender.Send(new GetAdminInflowQuery { Params = @params }, cancellation);
            return HandleResponse(response);
        }

        [HttpGet("outflows")]
        [ProducesResponseType<BaseResponse<PagedList<AdminTransactionModel>>>(200)]
        [SwaggerOperation(Summary = "Get Admin Outflow Transactions")]
        public async Task<IActionResult> GetAdminOutflowTransactions([FromQuery] MinimalParams @params, CancellationToken cancellation)
        {
            BaseResponse<PagedList<AdminTransactionModel>> response = await _sender.Send(new GetAdminOutflowQuery { Params = @params }, cancellation);
            return HandleResponse(response);
        }
    }
}
