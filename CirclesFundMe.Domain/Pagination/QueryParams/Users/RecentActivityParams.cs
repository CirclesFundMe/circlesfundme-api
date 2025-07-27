namespace CirclesFundMe.Domain.Pagination.QueryParams.Users
{
    public record RecentActivityParams : BaseParam
    {
        public RecentActivityTypeEnums Type { get; set; }
    }
}
