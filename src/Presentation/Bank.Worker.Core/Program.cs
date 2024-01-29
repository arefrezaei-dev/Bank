using Bank.Worker.Core.Registries;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;


await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((ctx, builder) =>
    {
        builder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.RegisterInfrastructure(hostContext.Configuration);
    })
    .Build()
    .RunAsync();




