namespace CirclesFundMe.Domain.Entities.Users
{
    public record CFMAccount : BaseEntity
    {
        public required string Name { get; set; }
        public AccountTypeEnums AccountType { get; set; }
        public AccountStatusEnums AccountStatus { get; set; }

        public ICollection<AppUser> Users { get; set; } = [];
    }
}
