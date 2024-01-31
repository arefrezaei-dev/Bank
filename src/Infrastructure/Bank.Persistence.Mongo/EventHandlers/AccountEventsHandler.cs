using Bank.Api.Common.Queries;
using Bank.Domain.IntegrationEvents;
using Bank.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.Middleware;
using System.Threading;

namespace Bank.Persistence.Mongo.EventHandlers
{
    public class AccountEventsHandler :
        IConsumer<AccountCreated>
    {
        private readonly IQueryDbContext _db;
        private readonly IAggregateRepository<Customer, Guid> _customersRepo;
        private readonly IAggregateRepository<Account, Guid> _accountsRepo;
        private readonly ILogger<AccountEventsHandler> _logger;

        public AccountEventsHandler(
            IQueryDbContext db,
            IAggregateRepository<Customer, Guid> customersRepo,
            IAggregateRepository<Account, Guid> accountsRepo,
            ILogger<AccountEventsHandler> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _customersRepo = customersRepo ?? throw new ArgumentNullException(nameof(customersRepo));
            _accountsRepo = accountsRepo ?? throw new ArgumentNullException(nameof(accountsRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Consume(ConsumeContext<AccountCreated> context)
        {
            _logger.LogInformation("updating details for account {AccountId} ...", context.Message.AccountId);

            var accountView = await BuildAccountViewAsync(context.Message.AccountId, context.CancellationToken);
            await UpsertAccountViewAsync(accountView, context.CancellationToken);
        }


        public async Task Handle(TransactionHappened @event, CancellationToken cancellationToken)
        {
            _logger.LogInformation("processing transaction on account {AccountId} ...", @event.AccountId);

            var accountView = await BuildAccountViewAsync(@event.AccountId, cancellationToken);
            await UpsertAccountViewAsync(accountView, cancellationToken);
        }

        private async Task<AccountDetails> BuildAccountViewAsync(Guid accountId, CancellationToken cancellationToken)
        {
            var account = await _accountsRepo.RehydrateAsync(accountId, cancellationToken);
            var customer = await _customersRepo.RehydrateAsync(account.OwnerId, cancellationToken);

            var accountView = new AccountDetails(account.Id,
                account.OwnerId, customer.FirstName, customer.LastName, customer.Email.Value,
                account.Balance);
            return accountView;
        }

        private async Task UpsertAccountViewAsync(AccountDetails accountView, CancellationToken cancellationToken)
        {
            var filter = Builders<AccountDetails>.Filter
                .Eq(a => a.Id, accountView.Id);

            var update = Builders<AccountDetails>.Update
                .Set(a => a.Id, accountView.Id)
                .Set(a => a.OwnerFirstName, accountView.OwnerFirstName)
                .Set(a => a.OwnerLastName, accountView.OwnerLastName)
                .Set(a => a.OwnerEmail, accountView.OwnerEmail)
                .Set(a => a.OwnerId, accountView.OwnerId)
                .Set(a => a.Balance, accountView.Balance);

            await _db.AccountsDetails.UpdateOneAsync(filter,
                cancellationToken: cancellationToken,
                update: update,
                options: new UpdateOptions() { IsUpsert = true });

            _logger.LogInformation("updated details for account {AccountId}", accountView.Id);
        }

    }
}
