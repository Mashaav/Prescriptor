using Microsoft.EntityFrameworkCore;
using Prescriptor.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prescriptor.Data
{
    public class PrescriptorContext : DbContext
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
