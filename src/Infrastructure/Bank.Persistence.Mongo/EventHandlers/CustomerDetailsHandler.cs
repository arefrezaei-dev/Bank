using Amazon.Runtime.Internal.Util;
using Bank.Api.Common.Queries;
using Bank.Domain;
using Bank.Domain.DomainServices;
using Bank.Domain.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.Mongo.EventHandlers
{
    public class CustomerDetailsHandler : INotificationHandler<CustomerCreated>
    {
        #region Fields
        private readonly ILogger<CustomerDetailsHandler> _logger;
        private readonly IAggregateRepository<Customer, Guid> _customersRepo;
        private readonly IAggregateRepository<Account, Guid> _accountsRepo;
        private readonly ICurrencyConverter _currencyConverter;
        private readonly IQueryDbContext _db;
        #endregion

        #region Constructor
        public CustomerDetailsHandler(ILogger<CustomerDetailsHandler> logger,
            IAggregateRepository<Customer, Guid> customersRepo,
            IAggregateRepository<Account, Guid> accountsRepo,
            ICurrencyConverter currencyConverter,
            IQueryDbContext db)
        {
            _logger = logger;
            _customersRepo = customersRepo;
            _accountsRepo = accountsRepo;
            _currencyConverter = currencyConverter;
            _db = db;
        }
        #endregion

        #region Handlers
        public async Task Handle(CustomerCreated @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("creating customer details for customer {CustomerId} ...", @event.CustomerId);

            var customerView = await BuildCustomerViewAsync(@event.CustomerId, cancellationToken);
            await SaveCustomerViewAsync(customerView, cancellationToken);
        }
        #endregion

        #region PrivateMethods
        private async Task<CustomerDetails> BuildCustomerViewAsync(Guid customerId, CancellationToken cancellationToken)
        {
            var customer = await _customersRepo.RehydrateAsync(customerId, cancellationToken);

            var totalBalance = Money.Zero(Currency.CanadianDollar);
            var accounts = new CustomerAccountDetails[customer.Accounts.Count];

            int index = 0;
            foreach (var id in customer.Accounts)
            {
                var account = await _accountsRepo.RehydrateAsync(id, cancellationToken);
                accounts[index++] = CustomerAccountDetails.Map(account);

                totalBalance = totalBalance.Add(account.Balance, _currencyConverter);
            }
            var customerView = new CustomerDetails(customer.Id, customer.FirstName, customer.LastName, customer.Email.Value, accounts, totalBalance);
            return customerView;
        }

        private async Task SaveCustomerViewAsync(CustomerDetails customerView, CancellationToken cancellationToken)
        {
            var filter = Builders<CustomerDetails>.Filter.Eq(a => a.Id, customerView.Id);

            var update = Builders<CustomerDetails>.Update
                .Set(a => a.Id, customerView.Id)
                .Set(a => a.FirstName, customerView.FirstName)
                .Set(a => a.LastName, customerView.LastName)
                .Set(a => a.Email, customerView.Email)
                .Set(a => a.Accounts, customerView.Accounts)
                .Set(a => a.TotalBalance, customerView.TotalBalance);

            await _db.CustomersDetails.UpdateOneAsync(filter,
                cancellationToken: cancellationToken,
                update: update,
                options: new UpdateOptions() { IsUpsert = true });

            _logger.LogInformation($"updated customer details for customer {customerView.Id}");
        }
        #endregion
    }
}
