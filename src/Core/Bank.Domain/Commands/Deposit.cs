using Bank.Domain.DomainServices;
using Bank.Domain.IntegrationEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Commands
{
    public record Deposit:IRequest
    {
        public Deposit(Guid accountId, Money amount)
        {
            AccountId = accountId;
            Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        }
        public Guid AccountId { get; }
        public Money Amount { get; }
    }

    public class DepositHandler : IRequestHandler<Deposit>
    {
        private readonly IAggregateRepository<Account, Guid> _accountEventsService;
        private readonly ICurrencyConverter _currencyConverter;
        public DepositHandler(IAggregateRepository<Account, Guid> accountEventsService, ICurrencyConverter currencyConverter)
        {
            _accountEventsService = accountEventsService;
            _currencyConverter = currencyConverter;
        }
        public async Task Handle(Deposit command, CancellationToken cancellationToken)
        {
            var account = await _accountEventsService.RehydrateAsync(command.AccountId);

            if (null == account)
                throw new ArgumentOutOfRangeException(nameof(Deposit.AccountId), "invalid account id");

            account.Deposit(command.Amount, _currencyConverter);

            await _accountEventsService.PersistAsync(account);

            var @event = new TransactionHappened(Guid.NewGuid(), account.Id);
            //await _eventProducer.DispatchAsync(@event, cancellationToken);
        }
    }
}
