using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.DomainServices
{
    public class FakeCurrencyConverter : ICurrencyConverter
    {
        public Money Convert(Money amount, Currency currency)
        {
            return amount.Currency == currency ? amount : new Money(currency, amount.Value);
        }
    }
}
