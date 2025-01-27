using CloudTrack.Registration.Domain.Common;

namespace CloudTrack.Registration.Domain.CompetitorRegistration;

public interface IRegistrationService : IDomainService
{
    Task RegisterAsync();
}
