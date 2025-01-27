using MassTransit;

namespace CloudTrack.Registration.Messaging;

[EntityName("registration-completed")]
public sealed record CompetitorRegisteredIntegrationEvent(
        Guid CompetitorId,
        Guid CompetitionId,
        string FirstName,
        string LastName,
        DateTime BirthDate,
        string City,
        string PhoneNumber,
        string ContactPersonNumber,
        string Number)
{ }
