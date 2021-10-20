using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace SimChargeWinApp.Models
{
    public partial class EmployeeRequestDBContext : DbContext
    {
        public EmployeeRequestDBContext()
        {
        }

        public EmployeeRequestDBContext(DbContextOptions<EmployeeRequestDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblSimCharge> TblSimCharges { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("server=192.168.10.250;database=EmployeeRequestDB;User Id=EmplyUser2;Password=S33@||;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Persian_100_CI_AS");

            modelBuilder.Entity<TblSimCharge>(entity =>
            {
                entity.ToTable("Tbl_SimCharge");

                entity.Property(e => e.DateTime).HasColumnType("datetime");

                entity.Property(e => e.Value).HasMaxLength(150);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
