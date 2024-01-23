using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;

namespace Bank.Persistence.Mongo
{
    public record MongoConfig(string ConnectionString, string QueryDbName);
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMongoDb(this IServiceCollection services, MongoConfig configuration)
        {
            return services.AddSingleton(ctx => new MongoClient(connectionString: configuration.ConnectionString))
                .AddSingleton(ctx =>
                {
                    var client = ctx.GetRequiredService<MongoClient>();
                    var database = client.GetDatabase(configuration.QueryDbName);
                    return database;
                }).AddSingleton<IQueryDbContext, QueryDbContext>();
        }
    }
}
