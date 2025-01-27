using CloudTrack.Registration.Domain.CompetitionIntegration;
using CloudTrack.Registration.Domain.Competitors;
using FluentValidation;
using MassTransit;

namespace CloudTrack.Registration.Application.CompetitorRegistration;

internal class RegistrationService(
    IValidator<RegistrationRequestDto> registrationRequestDtoValidator,
    ISendEndpointProvider sendEndpointProvider,
    ICompetitionRepository competitionRepository,
    ICompetitorRepository competitorRepository) : IRegistrationService
{
    private readonly IValidator<RegistrationRequestDto> _registrationRequestDtoValidator = registrationRequestDtoValidator;
    private readonly ISendEndpointProvider _sendEndpointProvider = sendEndpointProvider;
    private readonly ICompetitionRepository _competitionRepository = competitionRepository;
    private readonly ICompetitorRepository _competitorRepository = competitorRepository;

    public async Task<Guid> RegisterAsync(RegistrationRequestDto dto)
    {
        await _registrationRequestDtoValidator.ValidateAndThrowAsync(dto);

        var competitionId = CompetitionId.From(dto.CompetitionId);

        var competition =  await _competitionRepository.GetAsync(competitionId)
            ?? throw new Common.Exceptions.NotFoundException("Competition does not exist");

        if (!competition.IsRegistrationOpen) throw new Common.Exceptions.ValidationException("Registration is closed");

        var numberOfRegistrations = await _competitorRepository.GetNumberOfRegisteredCompetitorsAsync(competitionId);
        if(numberOfRegistrations >= competition.MaxCompetitors) throw new Common.Exceptions.ValidationException("Registrations have reached the limit");

        var requestId = Guid.NewGuid();
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:register-competitor"));
        await endpoint.Send(new RegisterCompetitor(
            requestId,
            dto.CompetitionId,
            dto.FirstName,
            dto.LastName,
            dto.BirthDate,
            dto.City,
            dto.PhoneNumber,
            dto.ContactPersonNumber));

        return requestId;
    }
}
