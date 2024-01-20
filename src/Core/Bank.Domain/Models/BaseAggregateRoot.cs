using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Models
{
    public abstract record BaseAggregateRoot<TA, TKey> : BaseEntity<TKey>, IAggregateRoot<TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        private readonly Queue<IDomainEvent<TKey>> _events = new Queue<IDomainEvent<TKey>>();
        protected BaseAggregateRoot() { }
        protected BaseAggregateRoot(TKey id) : base(id)
        {
        }
        public long Version { get; private set; }

        public IReadOnlyCollection<IDomainEvent<TKey>> Events => _events.ToImmutableArray();

        public void ClearEvents()
        {
            _events.Clear();
        }

        /// <summary>
        /// updates the version field each time an event is applied
        /// </summary>
        /// <param name="event"></param>
        protected void Append(IDomainEvent<TKey> @event)
        {
            _events.Enqueue(@event);

            this.When(@event);

            this.Version++;
        }
        /// <summary>
        /// each overload of when() is intend to read almost expressing what business rules should apply and how the state should change when each 
        /// type of event occurs
        /// </summary>
        /// <param name="event"></param>
        protected abstract void When(IDomainEvent<TKey> @event);
    }
}
