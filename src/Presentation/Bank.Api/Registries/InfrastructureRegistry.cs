using Bank.Domain.Commands;
using Bank.Domain.DomainEvents;
using Bank.Domain.DomainServices;
using Bank.Domain.EventBus;
using Bank.Domain.IntegrationEvents;
using Bank.Persistence.Mongo;
using Bank.Persistence.Mongo.EventHandlers;
using Bank.Persistence.SQLServer;
using Bank.Persistence.SQLServer.EventSourcing;
using Bank.Transport.RabbitMQ;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace Bank.Api.Registries
{
    public record Infrastructure(string EventBus, string AggregateStore, string QueryDb);

    public static class InfrastructureRegistry
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            var infraConfig = config.GetSection("infrastructure").Get<Infrastructure>();
            return services
                .RegisterAggregateStore(config, infraConfig)
                .RegisterQueryDb(config, infraConfig)
                .RegisterEventBus(config, infraConfig);
        }
        private static IServiceCollection RegisterAggregateStore(this IServiceCollection services, IConfiguration config, Infrastructure infraConfig)
        {
            if (infraConfig.AggregateStore == "SQLServer")
            {
                var sqlConnString = config.GetConnectionString("sql");
                services.AddSQLServerPersistence(sqlConnString)
                    .AddDbContextPool<CustomerDbContext>(builder =>
                    {
                        builder.UseSqlServer(sqlConnString, opts =>
                        {
                            opts.EnableRetryOnFailure();
                        });
                    }).AddTransient<ICustomerEmailsService, SQLCustomerEmailsService>();
            }
            return services;
        }
        private static IServiceCollection RegisterEventBus(this IServiceCollection services, IConfiguration config, Infrastructure infraConfig)
        {
            //services.AddMassTransit(x =>
            //{
            //    x.UsingRabbitMq((ctx, conf) =>
            //    {

            //        conf.Host("localhost", 5672, "/", h =>
            //        {
            //            h.Username("guest");
            //            h.Password("guest");
            //        });
            //        conf.ConfigureEndpoints(ctx);
            //    });
            //});
            var rabbitOptions = config.GetSection("RabbitMQSettings").Get<RabbitMqOptions>();

            if (infraConfig.EventBus == "RabbitMQ")
            {
                services.AddMassTransit(config =>
                 {
                     config.SetKebabCaseEndpointNameFormatter();

                     config.UsingRabbitMq((ctx, conf) =>
                     {
                         conf.Host(rabbitOptions.HostName, rabbitOptions.Port ?? 5672, "/", h =>
                         {
                             h.Username(rabbitOptions.UserName);
                             h.Password(rabbitOptions.Password);
                         });
                         conf.ConfigureEndpoints(ctx);
                     });
                 });
                services.AddTransient<IEventBus, EventBus>();
            }

            else throw new ArgumentOutOfRangeException($"invalid event bus type: {infraConfig.EventBus}");

            return services;
        }
        public static IServiceCollection ConfigureMediatRPipelines(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
                typeof(CreateCustomer).Assembly));

            services.AddSingleton<IEventSerializer>(new JsonEventSerializer(new[]
            {
                typeof(CustomerEvents.CustomerCreated).Assembly
            }));

            return services;
        }
        public static IServiceCollection RegisterQueryDb(this IServiceCollection services, IConfiguration config, Infrastructure infraConfig)
        {
            if (infraConfig.QueryDb == "MongoDb")
            {
                var mongoConnStr = config.GetConnectionString("mongo");
                var mongoQueryDbName = config["queryDbName"];
                var mongoConfig = new MongoConfig(mongoConnStr, mongoQueryDbName);
                services.AddMongoDb(mongoConfig);
            }

            else throw new ArgumentOutOfRangeException($"invalid read db type: {infraConfig.QueryDb}");

            return services;
        }
    }
}
