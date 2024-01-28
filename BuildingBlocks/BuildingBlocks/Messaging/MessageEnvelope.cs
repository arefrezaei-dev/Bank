using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace BuildingBlocks.Messaging
{
    // Ref: https://www.enterpriseintegrationpatterns.com/patterns/messaging/EnvelopeWrapper.html
    public class MessageEnvelope
    {
        public MessageEnvelope(object? message, IDictionary<string, object?>? headers = null)
        {
            Message = message;
            Headers = headers ?? new Dictionary<string, object?>();
        }
        public object? Message { get; init; }
        public IDictionary<string, object?> Headers { get; init; }
    }
    public class MessageEnvelope<TMessage> : MessageEnvelope
    where TMessage : class, IMessage
    {
        public MessageEnvelope(TMessage message, IDictionary<string, object?> headers)
            : base(message, headers)
        {
            Message = message;
        }
        public new TMessage? Message { get; }
    }
}
