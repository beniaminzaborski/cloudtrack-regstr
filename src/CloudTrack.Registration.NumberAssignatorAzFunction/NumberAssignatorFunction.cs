using Azure.Messaging.ServiceBus;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Logging;
using MassTransit;
using CloudTrack.Registration.NumberAssignatorAzFunction.Consumers;
using Microsoft.Azure.Functions.Worker;

namespace CloudTrack.Registration.NumberAssignatorAzFunction;

public class NumberAssignatorFunction(
    IMessageReceiver receiver,
    ILogger<NumberAssignatorFunction> logger)
{
    public const string QueueName = "register-competitor";
    readonly IMessageReceiver _receiver = receiver;
    readonly ILogger<NumberAssignatorFunction> _logger = logger;

    [Function("NumberAssignatorFunction")]
    public async Task Run(
        [ServiceBusTrigger(QueueName, Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage receivedMessage,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("Run NumberAssignatorFunction");

        await _receiver.HandleConsumer<RegisterCompetitorConsumer>(QueueName, receivedMessage, cancellationToken);
    }
}
