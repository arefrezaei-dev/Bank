using Bank.Domain.IntegrationEvents;
using Bank.Domain.Models;
using Bank.Persistence.Mongo.EventHandlers;
using MassTransit;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bank.Worker.Core
{
    public class EventsConsumerWorker : BackgroundService
    {
        private readonly ILogger<EventsConsumerWorker> _logger;

        public EventsConsumerWorker(ILogger<EventsConsumerWorker> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    cfg.ReceiveEndpoint("order-created-event", e =>
                    {
                        //e.Consumer<CustomerDetailsHandler>();
                    });

                });

                await busControl.StartAsync(new CancellationToken());

                try
                {
                    Console.WriteLine("Press enter to exit");

                    await Task.Run(() => Console.ReadLine());

                    await Task.Delay(1000, stoppingToken);
                }
                finally
                {
                    await busControl.StopAsync();
                }
            }
        }
    }
}
