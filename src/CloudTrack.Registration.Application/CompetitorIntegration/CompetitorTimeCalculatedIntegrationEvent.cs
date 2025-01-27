using System;

namespace CloudTrack.Telemetry.IntegrationEvents;

public sealed record CompetitorTimeCalculatedIntegrationEvent(
    Guid CompetitorId,
    TimeSpan NetTime)
{ }
