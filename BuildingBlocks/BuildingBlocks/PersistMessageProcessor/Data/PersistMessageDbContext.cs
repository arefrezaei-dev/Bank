using BuildingBlocks.PersistMessageProcessor.Data.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BuildingBlocks.PersistMessageProcessor.Data
{
    public class PersistMessageDbContext : DbContext, IPersistMessageDbContext
    {
        #region Fields
        private readonly ILogger<PersistMessageDbContext>? _logger;
        #endregion

        #region Constructor
        public PersistMessageDbContext(DbContextOptions<PersistMessageDbContext> options,
        ILogger<PersistMessageDbContext>? logger = null)
        : base(options)
        {
            _logger = logger;
        }
        #endregion
        public DbSet<PersistMessage> PersistMessages => Set<PersistMessage>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new PersistMessageConfiguration());
            base.OnModelCreating(builder);
            //builder.ToSnakeCaseTables();
        }

        public Task ExecuteTransactionalAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
