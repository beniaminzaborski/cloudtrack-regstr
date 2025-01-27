using CloudTrack.Registration.Application.CompetitorRegistration;
using CloudTrack.Registration.NumberAssignatorAzFunction;
using CloudTrack.Registration.NumberAssignatorAzFunction.Consumers;
using MassTransit;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using System;

[assembly: FunctionsStartup(typeof(Startup))]

namespace CloudTrack.Registration.NumberAssignatorAzFunction;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        builder.Services
            .AddScoped<ICompetitorService, CompetitorService>()
            .AddScoped<NumberAssignatorFunction>()
            .AddScoped<CompetitorNotificationFunction>()
            .AddMassTransitForAzureFunctions(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<ConsumerNamespace>();
                cfg.AddRequestClient<RegisterCompetitor>(new Uri($"queue:{NumberAssignatorFunction.QueueName}"));
            },
            "ServiceBusConnectionString");
    }
}
