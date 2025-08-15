namespace CirclesFundMe.Domain.RepositoryContracts.AdminPortal
{
    public interface IMessageTemplateRepository : IRepositoryBase<MessageTemplate>
    {
        Task<bool> HasTemplateForType(MessageTemplateType messageTemplateType);
        Task<MessageTemplate?> GetTemplateByTypeAsync(MessageTemplateType messageTemplateType, CancellationToken cancellationToken);
    }
}
