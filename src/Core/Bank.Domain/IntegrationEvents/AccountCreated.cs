using Bank.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.IntegrationEvents
{
    public record AccountCreated : IIntegrationEvent, INotification
    {
        public AccountCreated(Guid id, Guid accountId)
        {
            this.Id = id;
            this.AccountId = accountId;
        }

        public Guid AccountId { get; init; }
        public Guid Id { get; }
    }
}
