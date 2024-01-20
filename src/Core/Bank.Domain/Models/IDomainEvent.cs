using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Models
{
    public interface IDomainEvent<out TKey>
    {
        long AggregateVersion { get; }
        TKey AggregateId { get; }
        DateTime When { get; }
    }
}
