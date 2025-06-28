namespace CirclesFundMe.Application.Contants
{
    public record AppSettings
    {
        public required string FrontendBaseUrl { get; set; }
        public double IncomeToContributionPercentage { get; set; }
        public required MailSettings MailSettings { get; set; }
    }

    public record MailSettings
    {
        public required string SmtpFrom { get; set; }
        public required string SmtpServer { get; set; }
        public required string EmailUsername { get; set; }
        public required string EmailPassword { get; set; }
        public bool ActivateMailSending { get; set; }
    }

    public record CloudinarySettings
    {
        public required string CloudName { get; set; }
        public required string ApiKey { get; set; }
        public required string ApiSecret { get; set; }
    }
}
