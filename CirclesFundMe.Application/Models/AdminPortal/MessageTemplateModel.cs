using CirclesFundMe.Domain.Enums.AdminPortal;

namespace CirclesFundMe.Application.Models.AdminPortal
{
    public record MessageTemplateModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Body { get; set; }
        public MessageTemplateChannel Channel { get; set; }
        public MessageTemplateType Type { get; set; }
    }
}
