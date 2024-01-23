using Bank.Api.Common.Queries;
using MongoDB.Driver;

namespace Bank.Persistence.Mongo
{
    public class QueryDbContext : IQueryDbContext
    {
        private readonly IMongoDatabase _db;
        public QueryDbContext(IMongoDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            CustomerDetails = _db.GetCollection<CustomerDetails>("customerdetails");
        }

        public IMongoCollection<CustomerDetails> CustomerDetails { get; }
    }
}
