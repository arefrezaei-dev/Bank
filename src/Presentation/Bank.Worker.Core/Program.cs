using Bank.Domain.DomainEvents;
using Bank.Domain.DomainServices;
using Bank.Persistence.SQLServer.EventSourcing;
using Bank.Worker.Core.Registries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton<IEventSerializer>(new JsonEventSerializer(new[]
            {
                typeof(CustomerEvents.CustomerCreated).Assembly
            }));
        services.AddTransient<ICurrencyConverter, FakeCurrencyConverter>();
        services.RegisterInfrastructure(hostContext.Configuration);
    })
    .Build();
await host.RunAsync();
