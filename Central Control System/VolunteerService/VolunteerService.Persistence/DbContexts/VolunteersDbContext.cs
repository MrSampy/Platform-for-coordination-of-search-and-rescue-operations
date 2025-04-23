using Microsoft.EntityFrameworkCore;
using VolunteerService.Domain.Entities;

namespace VolunteerService.Persistence.DbContexts
{
    public class VolunteersDbContext : DbContext
    {
        public DbSet<Volunteer> Volunteers { get; set; }
        public DbSet<VolunteersDistricts> VolunteersDistricts { get; set; }
        public DbSet<VolunteersGroups> VolunteersGroups { get; set; }
        public DbSet<VolunteersEvents> VolunteersEvents { get; set; }
        public VolunteersDbContext(DbContextOptions options, bool ensureDeleted = false)
        : base(options)
        {
            Database.EnsureCreated();
            if (ensureDeleted)
            {
                Database.EnsureDeleted();
            }
        }
        public VolunteersDbContext(DbContextOptions options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Exclude BaseEntity from being mapped to a table
            modelBuilder.Ignore<BaseEntity>();

            // Volunteer Entity
            modelBuilder.Entity<Volunteer>(entity =>
            {
                entity.HasKey(v => v.GID);
                entity.Property(v => v.Name).IsRequired().HasMaxLength(100);
                entity.Property(v => v.Surname).IsRequired().HasMaxLength(100);
                entity.Property(v => v.SecondName).HasMaxLength(100);
                entity.Property(v => v.Email).IsRequired().HasMaxLength(255);
                entity.Property(v => v.MobilePhone).HasMaxLength(20);
                entity.Property(v => v.BirthDate).IsRequired();
                entity.Property(v => v.UserGID).IsRequired();
            });

            // VolunteersEvents Entity (Many-to-Many)
            modelBuilder.Entity<VolunteersEvents>(entity =>
            {
                entity.HasKey(vd => vd.GID);
                entity.Property(vd => vd.GID).ValueGeneratedNever();
                entity.HasIndex(vd => new { vd.VolunteerGID, vd.EventGID }).IsUnique();

                entity.HasOne<Volunteer>()
                      .WithMany()
                      .HasForeignKey(vd => vd.VolunteerGID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // VolunteersDistricts Entity (Many-to-Many)
            modelBuilder.Entity<VolunteersDistricts>(entity =>
            {
                entity.HasKey(vd => vd.GID);
                entity.Property(vd => vd.GID).ValueGeneratedNever();
                entity.HasIndex(vd => new { vd.VolunteerGID, vd.DistrictGID }).IsUnique();

                entity.HasOne<Volunteer>()
                      .WithMany()
                      .HasForeignKey(vd => vd.VolunteerGID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            // VolunteersGroups Entity (Many-to-Many)
            modelBuilder.Entity<VolunteersGroups>(entity =>
            {
                entity.HasKey(vg => vg.GID);
                entity.Property(vg => vg.GID).ValueGeneratedNever();
                entity.HasIndex(vg => new { vg.VolunteerGID, vg.GroupGID }).IsUnique();

                entity.HasOne<Volunteer>()
                      .WithMany()
                      .HasForeignKey(vg => vg.VolunteerGID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
