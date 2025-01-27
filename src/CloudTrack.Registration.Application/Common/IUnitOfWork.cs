namespace CloudTrack.Registration.Application.Common;

public interface IUnitOfWork : IDisposable
{
    Task BeginTransactionAsync();

    Task CommitAsync();

    Task RollbackAsync();
}
