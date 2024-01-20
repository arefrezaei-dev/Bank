using Bank.Domain.Common;
using Bank.Domain.DomainEvents;
using Bank.Domain.DomainServices;
using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain
{
    public record Account : BaseAggregateRoot<Account, Guid>
    {
        #region Constructore
        public Account() { }
        public Account(Guid id, Customer owner, Currency currency) : base(id)
        {
            if (owner == null)
                throw new ArgumentNullException(nameof(owner));
            if (currency == null)
                throw new ArgumentNullException(nameof(currency));

            this.Append(new AccountEvents.AccountCreated(this, owner, currency));
        }

        #endregion

        #region Properties
        public Guid OwnerId { get; private set; }
        public Money Balance { get; private set; }
        #endregion

        #region Public Methods
        public void Withdraw(Money amount, ICurrencyConverter currencyConverter)
        {
            if (amount.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "amount cannot be negative");

            var normalizedAmount = currencyConverter.Convert(amount, this.Balance.Currency);

            if (normalizedAmount.Value > this.Balance.Value)
                throw new Exception($"unable to withdrawn {normalizedAmount} from account {this.Id}");

            this.Append(new AccountEvents.Withdrawal(this, amount));
        }
        public void Deposit(Money amount, ICurrencyConverter currencyConverter)
        {
            if (amount.Value < 0)
                throw new ArgumentOutOfRangeException(nameof(amount), "amount cannot be negative");

            var normalizedAmount = currencyConverter.Convert(amount, this.Balance.Currency);

            this.Append(new AccountEvents.Deposit(this, normalizedAmount));
        }
        protected override void When(IDomainEvent<Guid> @event)
        {
            switch (@event)
            {
                case AccountEvents.AccountCreated c:
                    this.Id = c.AggregateId;
                    this.Balance = Money.Zero(c.Currency);
                    this.OwnerId = c.OwnerId;
                    break;
                case AccountEvents.Withdrawal w:
                    this.Balance = this.Balance.Subtract(w.Amount);
                    break;
                case AccountEvents.Deposit d:
                    this.Balance = this.Balance.Add(d.Amount);
                    break;
            }
        }
        public static Account Create(Guid accountId, Customer owner, Currency currency)
        {
            var account = new Account(accountId, owner, currency);
            owner.AddAccount(account);
            return account;
        }
        #endregion
    }
}
