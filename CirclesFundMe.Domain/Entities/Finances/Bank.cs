namespace CirclesFundMe.Domain.Entities.Finances
{
    public record Bank : BaseEntity
    {
        public required string Name { get; set; }
        public string? Slug { get; set; }
        public required string Code { get; set; }
    }
}
