using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildingBlocks.PersistMessageProcessor
{
    public interface IPersistMessageDbContext
    {
        DbSet<PersistMessage> PersistMessages { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default);
    }
}
