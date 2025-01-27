using CloudTrack.Registration.Domain.Common;
using CloudTrack.Registration.Domain.CompetitionIntegration;
using CloudTrack.Registration.Domain.Competitors;
using CloudTrack.Registration.Infrastructure.Persistence.Common;
using Microsoft.EntityFrameworkCore;

namespace CloudTrack.Registration.Infrastructure.Persistence.Repositories;

internal class CompetitorRepository(ApplicationDbContext dbContext) : Repository<Competitor, CompetitorId, ApplicationDbContext>(dbContext), ICompetitorRepository
{
    public async Task<IEnumerable<Competitor>> GetAllByCompetitionIdAsync(CompetitionId competitionId)
    {
        return await _dbContext.Set<Competitor>()
            .Where(c => c.CompetitionId.Equals(competitionId))
            .ToListAsync();
    }

    public async Task<int> GetNumberOfRegisteredCompetitorsAsync(CompetitionId competitionId)
    {
        return await _dbContext.Set<Competitor>()
            .Where(c => c.CompetitionId.Equals(competitionId))
            .CountAsync();
    }
}
