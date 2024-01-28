using Bank.Domain.IntegrationEvents;
using MassTransit;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Transport.RabbitMQ
{
    internal static class MassTransitExtensions
    {
        internal static void AddUserPublishers(this IRabbitMqBusFactoryConfigurator cfg)
        {
            cfg.Message<CustomerCreated>(e => e.SetEntityName($"{nameof(CustomerCreated)}.input_exchange")); // name of the primary exchange
            cfg.Publish<CustomerCreated>(e => e.ExchangeType = ExchangeType.Direct); // primary exchange type
            cfg.Send<CustomerCreated>(e =>
            {
                // route by message type to binding fanout exchange (exchange to exchange binding)
                e.UseRoutingKeyFormatter(context => context.Message.GetType().Name);
            });
        }
    }
}
