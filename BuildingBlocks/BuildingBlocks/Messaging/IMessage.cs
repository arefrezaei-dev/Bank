using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Messaging
{
    public interface IMessage: INotification
    {
        Guid MessageId { get; }
        DateTime Created { get; }
    }
}
