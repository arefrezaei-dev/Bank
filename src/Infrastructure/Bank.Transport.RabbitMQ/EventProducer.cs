using Bank.Domain;
using Bank.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Bank.Transport.RabbitMQ
{
    public class EventProducer : IEventProducer
    {
        #region Feilds
        private readonly ILogger<EventProducer> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        #endregion

        #region Constructor
        public EventProducer(ILogger<EventProducer> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }
        #endregion

        public async Task DispatchAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            if (null == @event)
                throw new ArgumentNullException(nameof(@event));

            _logger.LogInformation("publishing event {EventId} ...", @event.Id);
            var eventType = @event.GetType();

            var serialized = JsonSerializer.Serialize(@event, eventType);

            await _publishEndpoint.Publish(@event);
        }
    }
}
