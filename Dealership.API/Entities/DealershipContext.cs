using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Dealership.API.Entities
{
    public partial class DealershipContext : DbContext
    {
        public DealershipContext()
        {
        }

        public DealershipContext(DbContextOptions<DealershipContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Vehicle> Vehicles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Startup.DealershipConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.0-rtm-35687");

            modelBuilder.Entity<Vehicle>(entity =>
            {
                entity.Property(e => e.Id)
                    .HasMaxLength(24)
                    .ValueGeneratedNever();

                entity.Property(e => e.Color).HasMaxLength(30);

                entity.Property(e => e.Make)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
