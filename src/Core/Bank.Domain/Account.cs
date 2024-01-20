using Bank.Domain.Common;
using Bank.Domain.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain
{
    public class Account : BaseEntity<Guid>
    {
        #region Constructore
        public Account() { }
        //public Account(Guid id, Customer owner, Currency currency)
        //{
        //    if (owner == null)
        //        throw new ArgumentNullException(nameof(owner));
        //    if (currency == null)
        //        throw new ArgumentNullException(nameof(currency));

        //    //this.Append(new AccountEvents.AccountCreated(this, owner, currency));
        //}

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

            //if (normalizedAmount.Value > this.Balance.Value)
            //    throw new AccountTransactionException($"unable to withdrawn {normalizedAmount} from account {this.Id}", this);

            //this.Append(new AccountEvents.Withdrawal(this, amount));
        }
        #endregion
    }
}
