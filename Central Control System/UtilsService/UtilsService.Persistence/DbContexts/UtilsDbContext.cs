using Microsoft.EntityFrameworkCore;
using UtilsService.Domain.Entities;

namespace UtilsService.Persistence.DbContexts
{
    public class UtilsDbContext : DbContext
    {
        public DbSet<District> Districts { get; set; }
        public DbSet<MeasurementUnit> MeasurementUnits { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<ResourceMeasurementUnit> ResourceMeasurementUnits { get; set; }

        public UtilsDbContext(DbContextOptions options)
        : base(options)
        {

        }

        public UtilsDbContext(DbContextOptions options, bool ensureDeleted = false)
        : base(options)
        {
            Database.EnsureCreated();
            if (ensureDeleted)
            {
                Database.EnsureDeleted();
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure District entity
            modelBuilder.Entity<District>(entity =>
            {
                entity.HasKey(d => d.GID);
                entity.Property(d => d.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Configure MeasurementUnit entity
            modelBuilder.Entity<MeasurementUnit>(entity =>
            {
                entity.HasKey(mu => mu.GID);
                entity.Property(mu => mu.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Configure Resource entity
            modelBuilder.Entity<Resource>(entity =>
            {
                entity.HasKey(r => r.GID);
                entity.Property(r => r.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            // Configure ResourceMeasurementUnit entity
            modelBuilder.Entity<ResourceMeasurementUnit>(entity =>
            {
                entity.HasKey(rmu => rmu.GID);

                entity.HasOne<Resource>()
                      .WithMany()
                      .HasForeignKey(rmu => rmu.ResourceGID)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<MeasurementUnit>()
                      .WithMany()
                      .HasForeignKey(rmu => rmu.UnitGID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }

    }
}
