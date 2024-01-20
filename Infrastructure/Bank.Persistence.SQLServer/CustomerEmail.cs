using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.SQLServer
{
    public record CustomerEmail(Guid CustomerId, string Email);
}
