using Bank.Domain;
using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.SQLServer.EventSourcing
{
    public class SQLAggregateRepository<TA, TKey> : IAggregateRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        #region Fields

        #endregion

        #region Constructor

        #endregion

        #region Public Methods
        public async Task PersistAsync(TA aggregateRoot, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task RehydrateAsync(TKey key, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
