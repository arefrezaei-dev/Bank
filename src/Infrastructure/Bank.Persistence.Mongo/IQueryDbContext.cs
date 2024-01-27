using Bank.Api.Common.Queries;
using MongoDB.Driver;

namespace Bank.Persistence.Mongo
{
    public interface IQueryDbContext
    {
        IMongoCollection<AccountDetails> AccountsDetails { get; }
        IMongoCollection<CustomerDetails> CustomersDetails { get; }
        IMongoCollection<CustomerArchiveItem> Customers { get; }
    }
}
