namespace CirclesFundMe.Domain.Pagination.QueryParams.AdminPortal
{
    public record CommunicationParams : BaseParam
    {
        public CommunicationTarget Target { get; set; }
        public CommunicationChannel Channel { get; set; }
        public CommunicationStatus Status { get; set; }
    }
}
