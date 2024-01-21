using Bank.Domain.IntegrationEvents;
using MediatR;

namespace Bank.Domain.Commands
{
    public record CreateAccount:IRequest
    {
        public CreateAccount(Guid customerId, Guid accountId, Currency currency)
        {
            CustomerId = customerId;
            AccountId = accountId;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
        public Guid CustomerId { get; }
        public Guid AccountId { get; }
        public Currency Currency { get; }
    }
    public class CreateAccountHandler : IRequestHandler<CreateAccount>
    {
        private readonly IAggregateRepository<Customer, Guid> _customerEventsService;
        private readonly IAggregateRepository<Account, Guid> _accountEventsService;
        public CreateAccountHandler(IAggregateRepository<Customer, Guid> customerEventsService, IAggregateRepository<Account, Guid> accountEventsService)
        {
            _customerEventsService = customerEventsService;
            _accountEventsService = accountEventsService;
        }
        public async Task Handle(CreateAccount command, CancellationToken cancellationToken)
        {
            var customer = await _customerEventsService.RehydrateAsync(command.CustomerId);
            if (null == customer)
                throw new ArgumentOutOfRangeException(nameof(CreateAccount.CustomerId), "invalid customer id");

            var account = Account.Create(command.AccountId, customer, command.Currency);

            await _customerEventsService.PersistAsync(customer);
            await _accountEventsService.PersistAsync(account);

            var @event = new AccountCreated(Guid.NewGuid(), account.Id);
            //await _eventProducer.DispatchAsync(@event, cancellationToken);
        }
    }
}
