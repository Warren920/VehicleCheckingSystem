using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace VehicleTrackingSystem.DataModels
{
    public partial class VehicleTrackingSystemContext : DbContext
    {
        public VehicleTrackingSystemContext()
        {
        }

        public VehicleTrackingSystemContext(DbContextOptions<VehicleTrackingSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TireCondition> TireCondition { get; set; }
        public virtual DbSet<Vehicle> Vehicle { get; set; }
        public virtual DbSet<VehicleType> VehicleType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=VehicleTrackingSystem;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TireCondition>(entity =>
            {
                entity.HasKey(e => e.ConditionId);

                entity.HasIndex(e => e.Condition)
                    .IsUnique();

                entity.Property(e => e.ConditionId)
                    .HasColumnName("ConditionID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Condition)
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.HasKey(e => e.Vid);

                entity.HasIndex(e => e.Idnumber)
                    .IsUnique();

                entity.Property(e => e.Vid).HasColumnName("VId");

                entity.Property(e => e.ConditionId).HasColumnName("ConditionID");

                entity.Property(e => e.Idnumber)
                    .IsRequired()
                    .HasColumnName("IDNumber")
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.TypeId).HasColumnName("TypeID");

                entity.HasOne(d => d.TireCondition)
                    .WithMany(p => p.Vehicle)
                    .HasForeignKey(d => d.ConditionId)
                    .HasConstraintName("FK_Vehicle_TireCondition");

                entity.HasOne(d => d.VehicleType)
                    .WithMany(p => p.Vehicle)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Vehicle_VehicleType");
            });

            modelBuilder.Entity<VehicleType>(entity =>
            {
                entity.HasKey(e => e.TypeId);

                entity.HasIndex(e => e.TypeName)
                    .IsUnique();

                entity.Property(e => e.TypeId)
                    .HasColumnName("TypeID")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TypeName)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });
        }
    }
}
