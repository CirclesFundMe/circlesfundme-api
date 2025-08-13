namespace CirclesFundMe.Domain.RepositoryContracts.AdminPortal
{
    public interface ICommunicationRepository : IRepositoryBase<Communication>
    {
        Task<PagedList<CommunicationExtension>> GetCommunications(CommunicationParams @params, CancellationToken cancellationToken = default);
    }
}
