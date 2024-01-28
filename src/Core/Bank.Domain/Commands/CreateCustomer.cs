using Bank.Domain.DomainServices;
using Bank.Domain.EventBus;
using Bank.Domain.IntegrationEvents;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Commands
{
    public record CreateCustomer : IRequest
    {
        public CreateCustomer(Guid id, string firstName, string lastName, string email)
        {
            this.CustomerId = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }
        public Guid CustomerId { get; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Email { get; }
    }
    public class CreateCustomerHandler : IRequestHandler<CreateCustomer>
    {
        private readonly ICustomerEmailsService _customerEmailsService;
        private readonly IAggregateRepository<Customer,Guid> _eventsService;
        private readonly IEventBus _eventBus;

        public CreateCustomerHandler(ICustomerEmailsService customerEmailsService, IAggregateRepository<Customer, Guid> eventsService, IEventBus eventBus)
        {
            _customerEmailsService = customerEmailsService;
            _eventsService = eventsService;
            _eventBus = eventBus;
        }

        public async Task Handle(CreateCustomer command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                throw new Exception();
            if (await _customerEmailsService.ExistsAsync(command.Email))
                throw new Exception();

            var customer = Customer.Create(command.CustomerId, command.FirstName, command.LastName, command.Email);

            await _eventsService.PersistAsync(customer);
            await _customerEmailsService.CreateAsync(command.Email, customer.Id);

            //system crash or event bus can be unavailable 

            var @event = new CustomerCreated(Guid.NewGuid(),command.CustomerId);

            await _eventBus.PublishAsync(@event, cancellationToken);
        }
    }
}
