using CloudTrack.Registration.Domain.Common;
using CloudTrack.Registration.Domain.CompetitionIntegration;

namespace CloudTrack.Registration.Domain.Competitors;

public interface ICompetitorRepository : IRepository<Competitor, CompetitorId>
{
    Task<IEnumerable<Competitor>> GetAllByCompetitionIdAsync(CompetitionId competitionId);

    Task<int> GetNumberOfRegisteredCompetitorsAsync(CompetitionId competitionId);
}
