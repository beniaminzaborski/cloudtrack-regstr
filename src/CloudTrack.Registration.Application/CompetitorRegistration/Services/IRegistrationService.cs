using CloudTrack.Registration.Application.Common;

namespace CloudTrack.Registration.Application.CompetitorRegistration;

public interface IRegistrationService : IApplicationService
{
    Task<Guid> RegisterAsync(RegistrationRequestDto dto);
}
