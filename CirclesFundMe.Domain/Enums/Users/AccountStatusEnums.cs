namespace CirclesFundMe.Domain.Enums.Users
{
    public enum AccountStatusEnums
    {
        [Description("Pending")]
        Pending = 0,

        [Description("Active")]
        Active = 1,

        [Description("Inactive")]
        Inactive = 2,

        [Description("Suspended")]
        Suspended = 3,

        [Description("Deleted")]
        Deleted = 4
    }
}
