using AutoMapper;
using CloudTrack.Registration.Domain.CompetitionIntegration;
using CloudTrack.Registration.Domain.Competitors;

namespace CloudTrack.Registration.Application.Competitors;

internal class CompetitorService(
    ICompetitorRepository competitorRepository,
    IMapper mapper) : ICompetitorService
{
    private readonly ICompetitorRepository _competitorRepository = competitorRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CompetitorDto>> GetCompetitorsAsync(Guid competitionId)
    {
        var competitors = await _competitorRepository.GetAllByCompetitionIdAsync(CompetitionId.From(competitionId));
        return _mapper.Map<IEnumerable<Competitor>, IEnumerable<CompetitorDto>>(competitors);
    }
}
