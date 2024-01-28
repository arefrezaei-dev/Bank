using Bank.Domain.EventBus;
using Bank.Domain.Models;
using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Transport.RabbitMQ
{
    public class EventBus : IEventBus
    {
        #region Fields
        private readonly ILogger<EventBus> _logger;
        private readonly IPublishEndpoint _publishEndpoint;
        #endregion

        #region Constructor
        public EventBus(ILogger<EventBus> logger, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _publishEndpoint = publishEndpoint;
        }
        #endregion


        public async Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default)
        {
            if (null == @event)
                throw new ArgumentNullException(nameof(@event));

            _logger.LogInformation("publishing event {EventId} ...", @event.Id);

            var serialized = System.Text.Json.JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(serialized);

            await _publishEndpoint.Publish(@event, cancellationToken);
        }
    }
}
