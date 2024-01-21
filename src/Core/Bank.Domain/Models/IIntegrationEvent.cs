using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank.Domain.Models
{
    public interface IIntegrationEvent
    {
        Guid Id { get; }
    }
}
