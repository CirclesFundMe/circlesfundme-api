namespace CirclesFundMe.Infrastructure.Persistence.Repositories.Notifications
{
    public class NotificationRepository(DbSet<Notification> notifications, SqlDbContext context) : RepositoryBase<Notification>(notifications), INotificationRepository
    {
        private readonly DbSet<Notification> _notifications = notifications;
        private readonly SqlDbContext _context = context;

        public async Task<PagedList<Notification>> GetNotifications(string userId, NotificationParams @params, CancellationToken cancellation)
        {
            IQueryable<Notification> query = _notifications.AsNoTracking()
                .Where(n => n.UserId == userId && !n.IsDeleted);

            if (Enum.IsDefined(@params.NotificationTypeEnums))
            {
                query = query.Where(n => n.Type == @params.NotificationTypeEnums);
            }

            if (@params.IsRead.HasValue)
            {
                query = query.Where(n => n.IsRead == @params.IsRead.Value);
            }

            if (!string.IsNullOrWhiteSpace(@params.SearchKey))
            {
                query = query.Where(n => n.Title.Contains(@params.SearchKey));
            }

            int count = await query.CountAsync(cancellation);

            List<Notification> items = await query
                .OrderByDescending(n => n.CreatedDate)
                .Skip(@params.PageSize * (@params.PageNumber - 1))
                .Take(@params.PageSize)
                .Select(x => new Notification
                {
                    Id = x.Id,
                    Title = x.Title,
                    Type = x.Type,
                    CreatedDate = x.CreatedDate,
                    IsRead = x.IsRead,
                    ObjectIdentifier = x.ObjectIdentifier,
                    Metadata = x.Metadata
                })
                .ToListAsync(cancellation);

            return new PagedList<Notification>(items, count, @params.PageNumber, @params.PageSize);
        }

        public async Task<bool> MarkAllRead(string userId, CancellationToken cancellation)
        {
            IQueryable<Notification> notifications = _context.Notifications
                .Where(n => n.UserId == userId && !n.IsDeleted && !n.IsRead);

            if (!notifications.Any())
            {
                return false;
            }

            foreach (Notification notification in notifications)
            {
                notification.IsRead = true;
                notification.ReadAt = DateTime.UtcNow;
                notification.UpdateAudit(userId);
            }

            _context.Notifications.UpdateRange(notifications);
            int result = await _context.SaveChangesAsync(cancellation);

            return result > 0;
        }
    }
}
