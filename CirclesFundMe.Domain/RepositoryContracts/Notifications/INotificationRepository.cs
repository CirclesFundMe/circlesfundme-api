namespace CirclesFundMe.Domain.RepositoryContracts.Notifications
{
    public interface INotificationRepository : IRepositoryBase<Notification>
    {
        Task<PagedList<Notification>> GetNotifications(string userId, NotificationParams @params, CancellationToken cancellation);
        Task<bool> MarkAllRead(string userId, CancellationToken cancellation);
    }
}
