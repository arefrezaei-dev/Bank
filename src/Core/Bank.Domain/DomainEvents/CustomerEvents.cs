using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.DomainEvents
{
    public static class CustomerEvents
    {
        public record CustomerCreated : BaseDomainEvent<Customer, Guid>
        {
            /// <summary>
            /// for deserialization
            /// </summary>
            private CustomerCreated() { }
            public CustomerCreated(Customer customer, string firstname, string lastname, Email email) : base(customer)
            {
                FirstName = firstname;
                LastName = lastname;
                Email = email;
            }
            public string FirstName { get; init; }
            public string LastName { get; init; }
            public Email Email { get; init; }
        }
        public record AccountAdded : BaseDomainEvent<Customer, Guid>
        {
            private AccountAdded() { }
            public AccountAdded(Customer customer, Guid accountId) : base(customer)
            {
                AccountId = accountId;
            }
            public Guid AccountId { get; init; }
        }
    }
}
