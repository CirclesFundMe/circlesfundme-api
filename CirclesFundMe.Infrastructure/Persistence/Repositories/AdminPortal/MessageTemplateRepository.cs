namespace CirclesFundMe.Infrastructure.Persistence.Repositories.AdminPortal
{
    public class MessageTemplateRepository(SqlDbContext context) : RepositoryBase<MessageTemplate>(context.MessageTemplates), IMessageTemplateRepository
    {
        private readonly DbSet<MessageTemplate> _templates = context.MessageTemplates;

        public async Task<bool> HasTemplateForType(MessageTemplateType messageTemplateType)
        {
            return await _templates.AsNoTracking().AnyAsync(x => x.Type == messageTemplateType);
        }

        public async Task<MessageTemplate?> GetTemplateByTypeAsync(MessageTemplateType messageTemplateType, CancellationToken cancellationToken)
        {
            return await _templates.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Type == messageTemplateType, cancellationToken);
        }
    }
}
