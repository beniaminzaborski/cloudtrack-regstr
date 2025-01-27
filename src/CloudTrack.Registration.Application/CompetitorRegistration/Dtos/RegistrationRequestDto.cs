namespace CloudTrack.Registration.Application.CompetitorRegistration;

public sealed record RegistrationRequestDto(
    Guid CompetitionId,
    string FirstName,
    string LastName,
    DateTime BirthDate,
    string City,
    string PhoneNumber,
    string ContactPersonNumber)
{
}
