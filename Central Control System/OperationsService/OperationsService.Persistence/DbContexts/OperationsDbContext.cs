using Microsoft.EntityFrameworkCore;
using OperationsService.Domain.Entities;

namespace OperationsService.Persistence.DbContexts
{
    public class OperationsDbContext : DbContext
    {
        public DbSet<Event> Events { get; set; }
        public DbSet<EventStatus> EventStatuses { get; set; }
        public DbSet<EventType> EventTypes { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<OperationTask> OperationTasks { get; set; }
        public DbSet<OperationTaskStatus> OperationTaskStatuses { get; set; }
        public DbSet<OperationWorker> OperationWorkers { get; set; }
        public DbSet<ResourcesEvent> ResourcesEvents { get; set; }
        public DbSet<Message> Messages { get; set; }
        public OperationsDbContext(DbContextOptions options)
        : base(options)
        {

        }

        public OperationsDbContext(DbContextOptions options, bool ensureDeleted = false)
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
            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.GID);
                entity.HasOne<OperationWorker>()
                      .WithMany()
                      .HasForeignKey(re => re.From)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<OperationWorker>()
                      .WithMany()
                      .HasForeignKey(re => re.To)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.Property(e => e.Text)
                      .IsRequired()
                      .HasMaxLength(500);
                entity.Property(e => e.IsRead)
                        .IsRequired()
                        .HasDefaultValue(false);
                entity.HasOne<Event>()
                      .WithMany()
                      .HasForeignKey(re => re.EventGID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasKey(e => e.GID);
                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(e => e.Longitude)
                      .IsRequired()
                      .HasColumnType("decimal(18,8)");
                entity.Property(e => e.Latitude)
                      .IsRequired()
                      .HasColumnType("decimal(18,8)");
                entity.Property(e => e.Note)
                      .HasMaxLength(500);
            });

            modelBuilder.Entity<EventStatus>(entity =>
            {
                entity.HasKey(es => es.GID);
                entity.Property(es => es.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<EventType>(entity =>
            {
                entity.HasKey(et => et.GID);
                entity.Property(et => et.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(g => g.GID);
                entity.Property(g => g.Name)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.HasOne<Event>()
                      .WithMany()
                      .HasForeignKey(g => g.EventGID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OperationTask>(entity =>
            {
                entity.HasKey(ot => ot.GID);
                entity.Property(ot => ot.Name)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(ot => ot.TaskDescription)
                      .IsRequired()
                      .HasMaxLength(500);
                entity.HasOne<Group>()
                      .WithMany()
                      .HasForeignKey(ot => ot.GroupGID)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne<OperationTaskStatus>()
                      .WithMany()
                      .HasForeignKey(ot => ot.TaskStatusGID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<OperationTaskStatus>(entity =>
            {
                entity.HasKey(ots => ots.GID);
                entity.Property(ots => ots.Name)
                      .IsRequired()
                      .HasMaxLength(100);
            });

            modelBuilder.Entity<OperationWorker>(entity =>
            {
                entity.HasKey(ow => ow.GID);
                entity.Property(ow => ow.Name)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(ow => ow.Surname)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(ow => ow.SecondName)
                      .HasMaxLength(100);
                entity.Property(ow => ow.Email)
                      .IsRequired()
                      .HasMaxLength(150);
                entity.Property(ow => ow.IdentificationCode)
                      .IsRequired()
                      .HasMaxLength(50);
            });

            modelBuilder.Entity<ResourcesEvent>(entity =>
            {
                entity.HasKey(re => re.GID);
                entity.Property(re => re.RequiredQuantity)
                      .IsRequired()
                      .HasColumnType("decimal(18,8)");
                entity.Property(re => re.AvailableQuantity)
                      .IsRequired()
                      .HasColumnType("decimal(18,8)");
                entity.HasOne<Event>()
                      .WithMany()
                      .HasForeignKey(re => re.EventGID)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }

}
