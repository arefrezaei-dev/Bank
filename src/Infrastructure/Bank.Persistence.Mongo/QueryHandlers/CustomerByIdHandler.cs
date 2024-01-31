using Bank.Api.Common.Queries;
using MediatR;
using MongoDB.Driver;

namespace Bank.Persistence.Mongo.QueryHandlers
{
    public class CustomerByIdHandler : IRequestHandler<CustomerById, CustomerDetails>
    {
        private readonly IQueryDbContext _db;

        public CustomerByIdHandler(IQueryDbContext db)
        {
            _db = db;
        }

        public async Task<CustomerDetails> Handle(CustomerById request, CancellationToken cancellationToken)
        {
            var cursor = await _db.CustomersDetails.FindAsync(c => c.Id == request.customerId, null, cancellationToken);
            return await cursor.FirstOrDefaultAsync(cancellationToken);
        }
    }
}
