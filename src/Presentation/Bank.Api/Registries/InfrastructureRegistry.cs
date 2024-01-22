using Bank.Domain.Commands;
using Bank.Domain.DomainEvents;
using Bank.Domain.DomainServices;
using Bank.Persistence.SQLServer;
using Bank.Persistence.SQLServer.EventSourcing;
using BuildingBlocks.RabbitMQ;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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
            var rabbitOptions = config.GetSection("RabbitMQSettings").Get<RabbitMqOptions>();

            if (infraConfig.EventBus == "RabbitMQ")
            {
                services.AddMassTransit(config =>
                 {
                     config.UsingRabbitMq((ctx, conf) =>
                     {
                         conf.Host(rabbitOptions.HostName, rabbitOptions.Port ?? 5672, "/", h =>
                         {
                             h.Username(rabbitOptions.UserName);
                             h.Password(rabbitOptions.Password);
                         });
                     });
                 });
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
    }
}
