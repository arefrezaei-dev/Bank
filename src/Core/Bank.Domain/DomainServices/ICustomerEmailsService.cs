using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.DomainServices
{
    public interface ICustomerEmailsService
    {
        Task<bool> ExistsAsync(string email, CancellationToken cancellationToken = default);
        Task CreateAsync(string email, Guid customerId, CancellationToken cancellationToken = default);
    }
}
