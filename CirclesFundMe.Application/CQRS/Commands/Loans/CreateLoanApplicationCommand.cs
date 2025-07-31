namespace CirclesFundMe.Application.CQRS.Commands.Loans
{
    public record CreateLoanApplicationCommand : IRequest<BaseResponse<bool>>
    {
    }
}
