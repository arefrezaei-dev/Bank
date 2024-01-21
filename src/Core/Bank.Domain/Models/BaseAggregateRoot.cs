using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
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

        #region Factory

        private static readonly ConstructorInfo CTor;

        static BaseAggregateRoot()
        {
            var aggregateType = typeof(TA);
            CTor = aggregateType.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public,
                null, new Type[0], new ParameterModifier[0]);
            if (null == CTor)
                throw new InvalidOperationException($"Unable to find required private parameterless constructor for Aggregate of type '{aggregateType.Name}'");
        }

        public static TA Create(IEnumerable<IDomainEvent<TKey>> events)
        {
            if (null == events || !events.Any())
                throw new ArgumentNullException(nameof(events));
            var result = (TA)CTor.Invoke(new object[0]);

            var baseAggregate = result as BaseAggregateRoot<TA, TKey>;
            if (baseAggregate != null)
                foreach (var @event in events)
                    baseAggregate.Append(@event);

            result.ClearEvents();

            return result;
        }

        #endregion Factory
    }
}
