using Bank.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.EventBus
{
    public interface IEventBus
    {
        Task PublishAsync(IIntegrationEvent @event, CancellationToken cancellationToken = default);
    }
}
