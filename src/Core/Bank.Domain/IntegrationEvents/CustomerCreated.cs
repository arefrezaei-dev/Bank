using Bank.Domain.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.IntegrationEvents
{
    public record CustomerCreated : IIntegrationEvent, INotification
    {
        public CustomerCreated(Guid id, Guid customerId)
        {
            this.Id = id;
            this.CustomerId = customerId;
        }
        public Guid Id { get; }
        public Guid CustomerId { get; }
    }
}
