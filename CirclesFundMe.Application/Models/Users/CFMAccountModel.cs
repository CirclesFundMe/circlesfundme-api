namespace CirclesFundMe.Application.Models.Users
{
    public record AccountModel : BaseModel
    {
        public required string Name { get; set; }
        public AccountTypeEnums AccountType { get; set; }
        public AccountStatusEnums AccountStatus { get; set; }
    }
}
