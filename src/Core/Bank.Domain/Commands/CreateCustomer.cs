using MediatR;
using System;
using System.Collections.Generic;
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
        public async Task Handle(CreateCustomer command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
