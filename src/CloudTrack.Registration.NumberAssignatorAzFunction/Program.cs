using CloudTrack.Registration.Application.CompetitorRegistration;
using CloudTrack.Registration.NumberAssignatorAzFunction;
using CloudTrack.Registration.NumberAssignatorAzFunction.Consumers;
using MassTransit;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var host = new HostBuilder()
    .ConfigureFunctionsWorkerDefaults()
    .ConfigureServices(services =>
    {
        services
            .AddApplicationInsightsTelemetryWorkerService()
            .ConfigureFunctionsApplicationInsights()
            .AddScoped<ICompetitorService, CompetitorService>()
            .AddScoped<NumberAssignatorFunction>()
            .AddMassTransitForAzureFunctions(cfg =>
            {
                cfg.AddConsumersFromNamespaceContaining<ConsumerNamespace>();
                cfg.AddRequestClient<RegisterCompetitor>(new Uri($"queue:{NumberAssignatorFunction.QueueName}"));
            }, "ServiceBusConnectionString");
    })
    .Build();

host.Run();
