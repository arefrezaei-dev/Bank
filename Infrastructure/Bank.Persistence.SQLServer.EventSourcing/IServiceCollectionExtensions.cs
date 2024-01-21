using Bank.Domain;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.SQLServer.EventSourcing
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddSQLServerPersistence(this IServiceCollection services, string connectionString)
        {
            SqlMapper.AddTypeHandler(new ByteArrayTypeHandler());

            return services.AddSingleton(new SqlConnectionStringProvider(connectionString))
                           .AddSingleton<IAggregateTableCreator, AggregateTableCreator>()
                           .AddSingleton(typeof(IAggregateRepository<,>), typeof(SQLAggregateRepository<,>));
        }
    }
}
