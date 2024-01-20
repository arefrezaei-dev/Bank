using Bank.Domain.DomainServices;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.SQLServer
{
    public class SQLCustomerEmailsService : ICustomerEmailsService
    {
        private readonly CustomerDbContext _dbContext;

        public SQLCustomerEmailsService(CustomerDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task CreateAsync(string email, Guid customerId, CancellationToken cancellationToken = default)
        {
            await _dbContext.CustomerEmails.AddAsync(new CustomerEmail(customerId, email), cancellationToken)
                .ConfigureAwait(false);
            await _dbContext.SaveChangesAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default)
        => await _dbContext.CustomerEmails.AnyAsync(e => e.Email == email, cancellationToken);
    }
}
