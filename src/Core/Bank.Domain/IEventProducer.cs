using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain
{
    public interface IEventProducer
    {
        Task DispatchAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);
    }
}
