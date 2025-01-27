using CloudTrack.Registration.Application.Common;
using CloudTrack.Registration.Domain.Competitors;
using CloudTrack.Telemetry.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace CloudTrack.Registration.Application.CompetitorIntegration;

public class CompetitorConsumer(
    ILogger<CompetitorConsumer> logger,
    IUnitOfWork unitOfWork,
    ICompetitorRepository competitorRepository) : IConsumer<CompetitorTimeCalculatedIntegrationEvent>
{
    private readonly ILogger<CompetitorConsumer> _logger = logger;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICompetitorRepository _competitorRepository = competitorRepository;

    public async Task Consume(ConsumeContext<CompetitorTimeCalculatedIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation($"Time calculated for competitor id: {message.CompetitorId}");

        var competitor = await _competitorRepository.GetAsync(CompetitorId.From(message.CompetitorId));
        if (competitor is not null)
        {
            competitor.SetNetTime(message.NetTime);
            await _competitorRepository.UpdateAsync(competitor);
            await _unitOfWork.CommitAsync();
        }
    }
}

public class CompetitorConsumerDefinition : ConsumerDefinition<CompetitorConsumer>
{
    public CompetitorConsumerDefinition()
    {
        EndpointName = "competitor-time-calculated-events-to-registr-service";
    }
}
