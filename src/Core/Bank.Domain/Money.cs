using Bank.Domain.DomainServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain
{
    public record Money
    {
        #region Constructor
        public Money(Currency currency, decimal value)
        {
            Value = value;
            Currency = currency ?? throw new ArgumentNullException(nameof(currency));
        }
        #endregion

        #region Properties
        public decimal Value { get; }
        public Currency Currency { get; }
        #endregion

        #region Methods
        public Money Subtract(Money other, ICurrencyConverter converter = null)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));

            if (other.Currency != this.Currency)
            {
                if (converter is null)
                    throw new ArgumentNullException(nameof(converter), "Currency Converter is requried when currencies don't match");

                var converted = converter.Convert(other, this.Currency);
                return new Money(this.Currency, this.Value - converted.Value);
            }

            return new Money(this.Currency, this.Value - other.Value);
        }

        public Money Add(Money other, ICurrencyConverter converter = null)
        {
            if (other is null)
                throw new ArgumentNullException(nameof(other));
            if (other.Currency != this.Currency)
            {
                if (converter is null)
                    throw new ArgumentNullException(nameof(converter), "Currency Converter is requried when currencies don't match");

                var converted = converter.Convert(other, this.Currency);
                return new Money(this.Currency, this.Value + converted.Value);
            }
            return new Money(this.Currency, this.Value + other.Value);
        }

        public static Money Zero(Currency currency) => new Money(currency, 0);

        public override string ToString() => $"{Value} {Currency}";
        #endregion
    }
}
