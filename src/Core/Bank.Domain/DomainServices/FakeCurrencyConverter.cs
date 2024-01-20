using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.DomainServices
{
    public class FakeCurrencyConverter : ICurrencyConverter
    {
        /// <summary>
        /// domain service that accepts only micro types (micro types pattern)
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public Money Convert(Money amount, Currency currency)
        {
            return amount.Currency == currency ? amount : new Money(currency, amount.Value);
        }
    }
}
