using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Models
{
    public interface IAggregateRoot<out TKey> : IEntity<TKey>
    {
        /// <summary>
        /// keeps track of the aggregate's version
        /// resolving conflicts during writes or building the query models
        /// </summary>
        long Version { get; }
        IReadOnlyCollection<IDomainEvent<TKey>> Events { get; }
        void ClearEvents();
    }
}
