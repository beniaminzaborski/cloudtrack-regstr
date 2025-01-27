using CloudTrack.Registration.Domain.CompetitionIntegration;
using CloudTrack.Registration.Infrastructure.Persistence.Common;

namespace CloudTrack.Registration.Infrastructure.Persistence.Repositories;

internal class CompetitionRepository(ApplicationDbContext dbContext) : Repository<Competition, CompetitionId, ApplicationDbContext>(dbContext), ICompetitionRepository
{
}
