using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.PersistMessageProcessor
{
    [Flags]
    public enum MessageDeliveryType
    {
        Outbox = 1,
        Inbox = 2,
        Internal = 4
    }
}
