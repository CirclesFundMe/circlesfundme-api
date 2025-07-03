namespace CirclesFundMe.Application.CQRS.Queries.Contributions
{
    public record GetAutoFinanceBreakdownQuery : IRequest<BaseResponse<AutoFinanceBreakdownModel>>
    {
        public decimal CostOfVehicle { get; set; }
    }
}
