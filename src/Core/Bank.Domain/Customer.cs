using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain
{
    public record Customer : BaseAggregateRoot<Customer, Guid>
    {
        #region Constructor
        public Customer() { }
        public Customer(Guid id, string firstName, string lastName, Email email)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName));
            if (email is null)
                throw new ArgumentNullException(nameof(email));
            //this.Append(new CustomerEvents.CustomerCreated(this, firstname, lastname, email));
        }
        #endregion

        #region Properties
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        #endregion

        #region Public Methods
        public static Customer Create(Guid customerId, string firstName, string lastName, string email)
        {
            return new Customer(customerId, firstName, lastName, new Email(email));
        }
        #endregion
    }
}
