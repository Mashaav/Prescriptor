using BLL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data
{
    public partial class PrescriptorContext : DbContext
    {
        public PrescriptorContext(DbContextOptions<PrescriptorContext> options) : base(options)
        {
        }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Patient> Patients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prescription>().ToTable("Prescription");
            modelBuilder.Entity<Patient>().ToTable("Patient");
        }
    }
}

