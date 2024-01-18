using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Common
{
    public abstract class BaseEntity<TKey> : IEntity<TKey>
    {
        protected BaseEntity() { }
        protected BaseEntity(TKey id) => Id = id;

        public TKey Id { get; protected set; }
    }
}
