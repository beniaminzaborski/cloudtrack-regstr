using CloudTrack.Registration.Domain.Common;

namespace CloudTrack.Registration.Domain.CompetitionIntegration;

public record CompetitionId : EntityId<Guid>
{
    public CompetitionId(Guid value) : base(value) { }

    public static CompetitionId From(Guid value)
    { 
        return new CompetitionId(value);
    }
}
