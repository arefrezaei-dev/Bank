using Bank.Domain.DomainEvents;
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
        #region Fields
        private readonly HashSet<Guid> _accounts = new();

        #endregion
        #region Constructor
        public Customer() { }
        public Customer(Guid id, string firstName, string lastName, Email email) : base(id)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                throw new ArgumentNullException(nameof(firstName));
            if (string.IsNullOrWhiteSpace(lastName))
                throw new ArgumentNullException(nameof(lastName));
            if (email is null)
                throw new ArgumentNullException(nameof(email));
            this.Append(new CustomerEvents.CustomerCreated(this, firstName, lastName, email));
        }
        #endregion

        #region Properties
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public Email Email { get; private set; }
        public IReadOnlyCollection<Guid> Accounts => _accounts;

        #endregion

        protected override void When(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case CustomerEvents.CustomerCreated c:
                    this.Id = c.AggregateId;
                    this.FirstName = c.FirstName;
                    this.LastName = c.LastName;
                    this.Email = c.Email;
                    break;
                case CustomerEvents.AccountAdded aa:
                    _accounts.Add(aa.AccountId);
                    break;
            }
        }
        #region Public Methods
        public static Customer Create(Guid customerId, string firstName, string lastName, string email)
        {
            return new Customer(customerId, firstName, lastName, new Email(email));
        }
        public void AddAccount(Account account)
        {
            if (account is null)
                throw new ArgumentNullException(nameof(account));

            if (_accounts.Contains(account.Id))
                return;

            this.Append(new CustomerEvents.AccountAdded(this, account.Id));
        }
        #endregion
    }
}
