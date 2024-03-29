﻿using Bank.Domain;
using Bank.Domain.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Persistence.SQLServer.EventSourcing
{
    public class SQLAggregateRepository<TA, TKey> : IAggregateRepository<TA, TKey>
        where TA : class, IAggregateRoot<TKey>
    {
        #region Fields
        private readonly string _dbConnString;
        private readonly IAggregateTableCreator _tableCreator;
        private readonly IEventSerializer _eventSerializer;
        #endregion

        #region Constructor
        public SQLAggregateRepository(
           SqlConnectionStringProvider connectionStringProvider,
           IAggregateTableCreator tableCreator,
           IEventSerializer eventSerializer)
        {
            if (connectionStringProvider is null)
                throw new ArgumentNullException(nameof(connectionStringProvider));

            _dbConnString = connectionStringProvider.ConnectionString;
            _tableCreator = tableCreator ?? throw new ArgumentNullException(nameof(tableCreator));
            _eventSerializer = eventSerializer ?? throw new ArgumentNullException(nameof(eventSerializer));
        }
        #endregion

        #region Public Methods
        public async Task PersistAsync(TA aggregateRoot, CancellationToken cancellationToken = default)
        {
            if (aggregateRoot is null)
                throw new ArgumentNullException(nameof(aggregateRoot));
            if (!aggregateRoot.Events.Any())
                return;

            await _tableCreator.EnsureTableAsync<TA, TKey>(cancellationToken)
                               .ConfigureAwait(false);

            using var dbConn = new SqlConnection(_dbConnString);
            await dbConn.OpenAsync().ConfigureAwait(false);

            using var transaction = dbConn.BeginTransaction();

            try
            {
                var lastVersion = await this.GetLastAggregateVersionAsync(aggregateRoot, dbConn, transaction).ConfigureAwait(false);
                if (lastVersion >= aggregateRoot.Version)
                    throw new ArgumentOutOfRangeException(nameof(aggregateRoot), $"aggregate version mismatch, expected {aggregateRoot.Version}, got {lastVersion}");

                var tableName = _tableCreator.GetTableName<TA, TKey>();
                var sql = $@"INSERT INTO {tableName} (aggregateId, aggregateVersion, eventType, data, timestamp)
                         VALUES (@aggregateId, @aggregateVersion, @eventType, @data, @timestamp);";

                var entities = aggregateRoot.Events.Select(evt => AggregateEvent.Create(evt, _eventSerializer))
                                                   .ToList();
                await dbConn.ExecuteAsync(sql, param: entities, transaction: transaction)
                             .ConfigureAwait(false);

                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }

        }
        /// <summary>
        /// retrieving the current state of entity from events (stream aggregation process)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<TA> RehydrateAsync(TKey key, CancellationToken cancellationToken = default)
        {
            await _tableCreator.EnsureTableAsync<TA, TKey>(cancellationToken)
                               .ConfigureAwait(false);

            var tableName = _tableCreator.GetTableName<TA, TKey>();
            var sql = $@"SELECT aggregateId, aggregateVersion, eventType, data, timestamp
                         FROM {tableName}
                         WHERE aggregateId = @aggregateId
                         ORDER BY aggregateVersion ASC";

            using var dbConn = new SqlConnection(_dbConnString);
            await dbConn.OpenAsync().ConfigureAwait(false);

            //read all events for the specific stream by aggregateId
            var aggregateEvents = await dbConn.QueryAsync<AggregateEvent>(sql, new { aggregateId = key })
                                               .ConfigureAwait(false);
            if (aggregateEvents?.Any() == false)
                return null;

            var events = new List<IDomainEvent<TKey>>();

            foreach (var aggregateEvent in aggregateEvents)
            {
                var @event = _eventSerializer.Deserialize<TKey>(aggregateEvent.EventType, aggregateEvent.Data);
                events.Add(@event);
            }

            var result = BaseAggregateRoot<TA, TKey>.Create(events.OrderBy(e => e.AggregateVersion));
            return result;
        }
        #endregion

        #region PrivateMethods
        /// <summary>
        /// manage concurrency with last aggregate version
        /// </summary>
        /// <param name="aggregateRoot"></param>
        /// <param name="dbConn"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        private async Task<long?> GetLastAggregateVersionAsync(TA aggregateRoot, SqlConnection dbConn, IDbTransaction transaction)
        {
            var tableName = _tableCreator.GetTableName<TA, TKey>();
            var sql = @$"SELECT TOP 1 aggregateVersion
                         FROM {tableName} 
                         WHERE aggregateId = @aggregateId
                         ORDER BY aggregateVersion DESC";
            var result = await dbConn.QueryFirstOrDefaultAsync<long?>(sql, param: new { aggregateId = aggregateRoot.Id }, transaction: transaction)
                                      .ConfigureAwait(false);
            return result;
        }
        #endregion
    }
}
