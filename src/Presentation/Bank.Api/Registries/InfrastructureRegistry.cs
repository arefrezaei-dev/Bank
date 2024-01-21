using Bank.Domain.Commands;
using Bank.Domain.DomainEvents;
using Bank.Domain.DomainServices;
using Bank.Persistence.SQLServer;
using Bank.Persistence.SQLServer.EventSourcing;
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
            return services.RegisterAggregateStore(config, infraConfig);
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
        public static void ConfigureMediatRPipelines(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(
                typeof(CreateCustomer).Assembly));

            services.AddSingleton<IEventSerializer>(new JsonEventSerializer(new[]
            {
                typeof(CustomerEvents.CustomerCreated).Assembly
            }));
        }
    }
}
