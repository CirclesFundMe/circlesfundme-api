namespace CirclesFundMe.Application.Models.Contributions
{
    public record ContributionSchemeMiniModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
    }
}
