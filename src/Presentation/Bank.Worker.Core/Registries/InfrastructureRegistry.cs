using Bank.Domain.IntegrationEvents;
using Bank.Persistence.Mongo;
using Bank.Persistence.Mongo.EventHandlers;
using Bank.Persistence.SQLServer.EventSourcing;
using Bank.Transport.RabbitMQ;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Text.Json;

namespace Bank.Worker.Core.Registries
{
    public static class InfrastructureRegistry
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var infraConfig = configuration.GetSection("infrastructure").Get<InfrastructureConfig>();

            var rabbitOptions = configuration.GetSection("RabbitMQSettings").Get<RabbitMqOptions>();

            var mongoConnStr = configuration.GetConnectionString("mongo");

            var mongoQueryDbName = configuration["queryDbName"];

            var mongoConfig = new MongoConfig(mongoConnStr, mongoQueryDbName);

            return services.AddMongoDb(mongoConfig)
                .RegisterAggregateStore(configuration, infraConfig)
            .RegisterRabbitMQ(configuration, rabbitOptions);
        }

        private static IServiceCollection RegisterAggregateStore(this IServiceCollection services, IConfiguration config, InfrastructureConfig infraConfig)
        {
            if (infraConfig.AggregateStore == "SQLServer")
            {
                var sqlConnString = config.GetConnectionString("sql");
                services.AddSQLServerPersistence(sqlConnString);
            }
            else throw new ArgumentOutOfRangeException($"invalid aggregate store type: {infraConfig.AggregateStore}");

            return services;
        }

        private static IServiceCollection RegisterRabbitMQ(this IServiceCollection services, IConfiguration config, RabbitMqOptions rabbitMqOptions)
        {
            services.AddMassTransit(config =>
            {
                config.SetKebabCaseEndpointNameFormatter();

                config.AddConsumer<CustomerDetailsHandler>();

                config.UsingRabbitMq((ctx, conf) =>
                {
                    conf.Host(rabbitMqOptions.HostName, rabbitMqOptions.Port ?? 5672, "/", h =>
                    {
                        h.Username(rabbitMqOptions.UserName);
                        h.Password(rabbitMqOptions.Password);
                    });
                    conf.ReceiveEndpoint("send-customer-events", c =>
                    {
                        c.ConfigureConsumeTopology = false;
                        c.ConfigureConsumer<CustomerDetailsHandler>(ctx);
                        c.Bind("messages", e =>
                        {
                            e.RoutingKey = "customer-created";
                            e.ExchangeType = ExchangeType.Direct;
                        });
                    });
                });
            });
            services.AddScoped<CustomerDetailsHandler>();
            return services;
        }
    }
}
