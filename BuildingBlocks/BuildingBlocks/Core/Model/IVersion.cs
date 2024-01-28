using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Model
{
    // For handling optimistic concurrency
    public interface IVersion
    {
        long Version { get; set; }
    }
}
