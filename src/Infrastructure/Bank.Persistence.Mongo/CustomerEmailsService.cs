using Bank.Domain.DomainServices;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Bank.Persistence.Mongo
{
    public class CustomerEmailsService : ICustomerEmailsService
    {
        #region Fields
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<CustomerEmail> _coll;
        #endregion

        #region Constructor
        public CustomerEmailsService(IMongoDatabase db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _coll = _db.GetCollection<CustomerEmail>("CustomerEmails");
        }
        #endregion

        public async Task CreateAsync(string email, Guid customerId, CancellationToken cancellationToken = default)
        {
            var indexes = await (await _coll.Indexes.ListAsync()).ToListAsync(cancellationToken);
            if (!indexes.Any())
            {
                var indexKeys = Builders<CustomerEmail>.IndexKeys.Ascending(a => a.Email);
                var createIndex = new CreateIndexModel<CustomerEmail>(indexKeys, new CreateIndexOptions()
                {
                    Unique = true,
                    Name = "email"
                });
                await _coll.Indexes.CreateOneAsync(createIndex, cancellationToken: cancellationToken);
            }

            var update = Builders<CustomerEmail>.Update
                .Set(a => a.Id, customerId)
                .Set(a => a.Email, email);

            await _coll.UpdateOneAsync(c => c.Email == email, update, options: new UpdateOptions() { IsUpsert = true }, cancellationToken: cancellationToken);
        }

        public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
        {
            var filter = Builders<CustomerEmail>.Filter.Eq(e=>e.Email, email);

            var count = await _coll.CountDocumentsAsync(filter, new CountOptions()
            {
                Limit = 1
            }, cancellationToken: cancellationToken);

            return count > 0;
        }
    }
    internal class CustomerEmail
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public Guid Id { get; set; }
        public string Email { get; set; }
    }
}
