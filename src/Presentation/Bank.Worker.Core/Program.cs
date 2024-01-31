using Bank.Domain;
using Bank.Domain.DomainEvents;
using Bank.Domain.DomainServices;
using Bank.Persistence.Mongo.EventHandlers;
using Bank.Persistence.SQLServer.EventSourcing;
using Bank.Worker.Core.Registries;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


//await Host.CreateDefaultBuilder(args)
//    .ConfigureAppConfiguration((ctx, builder) =>
//    {
//        builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
//    })
//    .ConfigureServices((hostContext, services) =>
//    {
//        services.RegisterInfrastructure(hostContext.Configuration)
//        .RegisterWorker();

//    })
//    .Build()
//    .RunAsync();


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
