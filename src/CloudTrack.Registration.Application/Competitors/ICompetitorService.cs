using CloudTrack.Registration.Application.Common;

namespace CloudTrack.Registration.Application.Competitors;

public interface ICompetitorService : IApplicationService
{
    Task<IEnumerable<CompetitorDto>> GetCompetitorsAsync(Guid competitionId);
}
