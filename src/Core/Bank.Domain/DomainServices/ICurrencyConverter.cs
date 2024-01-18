using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.DomainServices
{
    public interface ICurrencyConverter
    {
        Money Convert(Money amount, Currency currency);
    }

}
