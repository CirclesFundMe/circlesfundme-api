namespace CirclesFundMe.Application.CQRS.Commands.Utility
{
    public record SendContactUsMailCommand : IRequest<BaseResponse<bool>>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Title { get; set; }
        public string? Message { get; set; }
    }
}
