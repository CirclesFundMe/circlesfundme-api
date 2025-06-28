namespace CirclesFundMe.Domain.Entities.Users
{
    public record UserDocument : BaseEntity
    {
        public UserDocumentTypeEnums DocumentType { get; set; }
        public string? DocumentUrl { get; set; }
        public string? DocumentName { get; set; }

        public string? UserId { get; set; }
        public virtual AppUser? User { get; set; }
    }
}
