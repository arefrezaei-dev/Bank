using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.SQLServer.EventSourcing
{
    public interface IAggregateTableCreator
    {
        Task EnsureTableAsync<TA, TKey>(CancellationToken cancellationToken = default)
            where TA : class, IAggregateRoot<TKey>;
        string GetTableName<TA, TKey>()
            where TA : class, IAggregateRoot<TKey>;
    }
}
