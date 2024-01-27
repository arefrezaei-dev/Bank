using Bank.Api.Common.Queries;
using Bank.Domain;
using MongoDB.Driver;

namespace Bank.Persistence.Mongo
{
    public class QueryDbContext : IQueryDbContext
    {
        private readonly IMongoDatabase _db;
        public QueryDbContext(IMongoDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            AccountsDetails = _db.GetCollection<AccountDetails>("accounts");
            CustomersDetails = _db.GetCollection<CustomerDetails>("customerdetails");
            Customers = _db.GetCollection<CustomerArchiveItem>("customers");
        }

        public IMongoCollection<AccountDetails> AccountsDetails { get; }
        public IMongoCollection<CustomerDetails> CustomersDetails { get; }
        public IMongoCollection<CustomerArchiveItem> Customers { get; }
    }
}
