namespace CirclesFundMe.Application.Contants
{
    public record EncryptionSettings
    {
        public string? Key { get; set; }
        public string? IV { get; set; }
    }
}
