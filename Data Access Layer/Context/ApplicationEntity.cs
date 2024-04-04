using Data_Access_Layer.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data_Access_Layer.Context
{
    public class ApplicationEntity : IdentityDbContext<ApplicationUser>
    {
        public ApplicationEntity() { }

        public ApplicationEntity(DbContextOptions<ApplicationEntity> options) : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Rate> Rates { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
                entity.Property(e => e.Id).HasColumnName("UserId");
                // Add any additional configuration for ApplicationUser entity
            });
        }
    }


}

