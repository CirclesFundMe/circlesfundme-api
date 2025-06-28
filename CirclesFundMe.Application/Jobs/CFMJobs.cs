namespace CirclesFundMe.Application.Jobs
{
    public class CFMJobs(EmailService emailService, IOptions<AppSettings> options)
    {
        private readonly EmailService _emailService = emailService;
        private readonly AppSettings _appSettings = options.Value;

        public async Task SendOTP(string emailAddress, string otp, string? firstName)
        {
            string userName = firstName ?? "Member";

            StringBuilder sb = new(_emailService.LoadHtmlTemplate("otp"));
            sb.Replace("{{FirstName}}", userName);
            sb.Replace("{{OTP}}", otp);
            sb.Replace("{{Year}}", DateTime.UtcNow.Year.ToString());

            EmailMessage msg = new(emailAddress, "Security Code", sb.ToString(), null);

            await _emailService.SendEmail(msg);
        }
    }
}
