namespace CirclesFundMe.Domain.Entities.AdminPortal
{
    public record MessageTemplate : BaseEntity
    {
        public string? Name { get; set; }
        public string? Body { get; set; }
        public MessageTemplateChannel Channel { get; set; }
        public MessageTemplateType Type { get; set; }
    }
}
