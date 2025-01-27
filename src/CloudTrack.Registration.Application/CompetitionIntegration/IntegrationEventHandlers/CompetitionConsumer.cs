using MassTransit;
using CloudTrack.Registration.Application.Common;
using CloudTrack.Registration.Domain.CompetitionIntegration;
using Microsoft.Extensions.Logging;
using CloudTrack.Competitions.Messaging;

namespace CloudTrack.Registration.Application.CompetitionIntegration;

public class CompetitionConsumer(
    ILogger<CompetitionConsumer> logger,
    IUnitOfWork unitOfWork,
    ICompetitionRepository competitionRepository) :
    IConsumer<CompetitionOpenedForRegistrationIntegrationEvent>,
    IConsumer<CompetitionRegistrationCompletedIntegrationEvent>,
    IConsumer<CompetitionMaxCompetitorsIncreasedIntegrationEvent>
{
    private readonly ILogger<CompetitionConsumer> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICompetitionRepository _competitionRepository = competitionRepository;

    public async Task Consume(ConsumeContext<CompetitionOpenedForRegistrationIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Registration is opened for competition Id: {message.Id} with max number of competitors {message.MaxCompetitors}");

        var competition = new Competition(CompetitionId.From(message.Id), message.MaxCompetitors, true);
        await _competitionRepository.CreateAsync(competition);
        await _unitOfWork.CommitAsync();
    }

    public async Task Consume(ConsumeContext<CompetitionRegistrationCompletedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Registration is closed for competition Id: {message.Id}");

        var competition = await _competitionRepository.GetAsync(CompetitionId.From(message.Id));
        if (competition is not null) 
        {
            competition.CloseRegistration();
            await _competitionRepository.UpdateAsync(competition);
            await _unitOfWork.CommitAsync();
        }
    }

    public async Task Consume(ConsumeContext<CompetitionMaxCompetitorsIncreasedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Max number of competitors for competition Id: {message.Id} has changed to {message.MaxCompetitors}");

        var competition = await _competitionRepository.GetAsync(CompetitionId.From(message.Id));
        if (competition is not null)
        {
            competition.IncreaseMaxCompetitorsTo(message.MaxCompetitors);
            await _competitionRepository.UpdateAsync(competition);
            await _unitOfWork.CommitAsync();
        }
    }
}

public class CompetitionConsumerDefinition : ConsumerDefinition<CompetitionConsumer>
{
    public CompetitionConsumerDefinition()
    {
        EndpointName = "competition-events-to-registration-service";
    }
}
