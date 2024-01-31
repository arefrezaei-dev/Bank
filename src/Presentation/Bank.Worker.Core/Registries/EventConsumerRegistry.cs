using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Worker.Core.Registries
{
    public static class EventConsumerRegistry
    {
        public static IServiceCollection RegisterWorker(this IServiceCollection services)
            => services.AddHostedService<EventsConsumerWorker>();
    }
}
