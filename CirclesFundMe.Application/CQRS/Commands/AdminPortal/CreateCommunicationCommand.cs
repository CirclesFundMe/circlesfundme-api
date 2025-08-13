namespace CirclesFundMe.Application.CQRS.Commands.AdminPortal
{
    public record CreateCommunicationCommand : IRequest<BaseResponse<bool>>
    {
        public string? Title { get; set; }
        public string? Body { get; set; }
        public CommunicationTarget Target { get; set; }
        public CommunicationChannel Channel { get; set; }
    }
}
