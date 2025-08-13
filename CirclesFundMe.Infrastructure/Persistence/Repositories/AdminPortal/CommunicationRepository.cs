namespace CirclesFundMe.Infrastructure.Persistence.Repositories.AdminPortal
{
    public class CommunicationRepository(SqlDbContext context) : RepositoryBase<Communication>(context.Communications), ICommunicationRepository
    {
        private readonly SqlDbContext _context = context;

        public async Task<PagedList<CommunicationExtension>> GetCommunications(CommunicationParams @params, CancellationToken cancellationToken = default)
        {
            IQueryable<Communication> query = _context.Communications.AsNoTracking();

            if (Enum.IsDefined(@params.Target))
            {
                query = query.Where(c => c.Target == @params.Target);
            }

            if (Enum.IsDefined(@params.Channel))
            {
                query = query.Where(c => c.Channel == @params.Channel);
            }

            if (Enum.IsDefined(@params.Status))
            {
                query = query.Where(c => c.Status == @params.Status);
            }

            if (!string.IsNullOrEmpty(@params.SearchKey))
            {
                query = query.Where(c => c.Title != null && c.Title.Contains(@params.SearchKey));
            }

            int totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(c => c.ScheduledAt)
                .Skip(@params.PageSize * (@params.PageNumber - 1))
                .Take(@params.PageSize)
                .Select(c => new CommunicationExtension
                {
                    Id = c.Id,
                    Title = c.Title,
                    Body = c.Body,
                    Target = c.Target,
                    Channel = c.Channel,
                    Status = c.Status,
                    ScheduledAt = c.ScheduledAt,
                    ProcessedAt = c.ProcessedAt,
                    ErrorMessage = c.ErrorMessage,
                    TotalRecipients = c.Recipients.Count()
                })
                .ToListAsync(cancellationToken);

            return new PagedList<CommunicationExtension>(items, totalCount, @params.PageNumber, @params.PageSize);
        }
    }
}
