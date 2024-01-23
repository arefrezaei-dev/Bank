using Bank.Api.Common.Queries;
using MongoDB.Driver;

namespace Bank.Persistence.Mongo
{
    public interface IQueryDbContext
    {
        IMongoCollection<CustomerDetails> CustomerDetails { get; }
    }
}
