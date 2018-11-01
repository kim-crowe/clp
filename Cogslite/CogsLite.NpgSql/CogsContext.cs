using Microsoft.EntityFrameworkCore;

namespace CogsLite.NpgSql
{
    public class CogsContext : DbContext
    {
        // ReSharper disable once VirtualMemberCallInConstructor
        public CogsContext(DbContextOptions<CogsContext> options) : base(options) =>
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

        public DbSet<Entities.UserEntity> Users { get; set; }

        public DbSet<Entities.GameEntity> Games { get; set; }

        public DbSet<Entities.CardEntity> Cards { get; set; }

        public DbSet<Entities.CardTypeEntity> CardTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ForNpgsqlUseSerialColumns();
            modelBuilder.ApplyConfiguration(new Entities.GameEntity());
            modelBuilder.ApplyConfiguration(new Entities.CardEntity());
            modelBuilder.ApplyConfiguration(new Entities.CardTypeEntity());
        }
    }
}