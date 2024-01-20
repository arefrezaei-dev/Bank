using Bank.Domain.DomainServices;
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

        public CreateCustomerHandler(ICustomerEmailsService customerEmailsService)
        {
            _customerEmailsService = customerEmailsService;
        }

        public async Task Handle(CreateCustomer command, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(command.Email))
                throw new Exception();
            if (await _customerEmailsService.ExistsAsync(command.Email))
                throw new Exception();

            var customer = Customer.Create(command.CustomerId, command.FirstName, command.LastName, command.Email);

            await _customerEmailsService.CreateAsync(command.Email, customer.Id);
        }
    }
}
