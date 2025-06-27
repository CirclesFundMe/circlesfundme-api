namespace CirclesFundMe.Application.Services
{
    public interface IQueueService
    {
        void EnqueueFireAndForgetJob<T>(Expression<Func<T, Task>> methodCall);
        void EnqueueDelayedJob<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay);
    }
    public record QueueService(ILogger<QueueService> logger) : IQueueService
    {
        private readonly ILogger<QueueService> _logger = logger;

        public void EnqueueDelayedJob<T>(Expression<Func<T, Task>> methodCall, TimeSpan delay)
        {
            string jobId = BackgroundJob.Schedule(methodCall, delay);
            _logger.LogInformation("Job {JobId} completed successfully", jobId);
        }

        public void EnqueueFireAndForgetJob<T>(Expression<Func<T, Task>> methodCall)
        {
            string jobId = BackgroundJob.Enqueue(methodCall);
            _logger.LogInformation("Job {JobId} completed successfully", jobId);
        }
    }
}
