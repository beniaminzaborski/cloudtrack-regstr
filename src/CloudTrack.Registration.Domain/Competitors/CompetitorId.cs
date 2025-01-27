using CloudTrack.Registration.Domain.Common;

namespace CloudTrack.Registration.Domain.Competitors;

public record CompetitorId : EntityId<Guid>
{
    public CompetitorId(Guid value) : base(value) { }

    public static CompetitorId From(Guid value)
    {
        return new CompetitorId(value);
    }
}
