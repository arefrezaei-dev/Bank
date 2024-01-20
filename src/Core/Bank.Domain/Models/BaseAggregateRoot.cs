using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Models
{
    public abstract record BaseAggregateRoot<TA, TKey> : BaseEntity<TKey>, IAggregateRoot<TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        public long Version => throw new NotImplementedException();

        public IReadOnlyCollection<IDomainEvent<TKey>> Events => throw new NotImplementedException();

        public void ClearEvents()
        {
            throw new NotImplementedException();
        }
    }
}
