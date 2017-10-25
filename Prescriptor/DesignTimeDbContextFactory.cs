using Microsoft.EntityFrameworkCore.Design;
using System.IO;
using DAL.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Prescriptor
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<PrescriptorContext>
    {
        public PrescriptorContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var builder = new DbContextOptionsBuilder<PrescriptorContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            builder.UseSqlServer(connectionString);
            return new PrescriptorContext(builder.Options);
        }
    }
}
