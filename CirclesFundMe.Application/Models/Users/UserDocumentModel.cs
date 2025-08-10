namespace CirclesFundMe.Application.Models.Users
{
    public record UserDocumentModel
    {
        public string? DocumentType { get; set; }
        public string? DocumentUrl { get; set; }
        public string? DocumentName { get; set; }
    }
}
