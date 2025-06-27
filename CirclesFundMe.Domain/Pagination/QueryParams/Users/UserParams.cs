namespace CirclesFundMe.Domain.Pagination.QueryParams.Users
{
    public record UserParams : BaseParam
    {
        public UserTypeEnums UserType { get; set; }
    }
}
