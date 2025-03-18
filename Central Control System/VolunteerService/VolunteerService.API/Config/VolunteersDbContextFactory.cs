using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VolunteerService.Domain.Exceptions;
using VolunteerService.Persistence.DbContexts;

namespace VolunteerService.API.Config
{
    public class VolunteersDbContextFactory : IDesignTimeDbContextFactory<VolunteersDbContext>
    {
        public VolunteersDbContext CreateDbContext(string[] args)
        {
            // Build configuration
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Gets current directory
                .AddJsonFile("appsettings.json") // Load appsettings.json
                .Build();

            // Read connection string
            string connectionString = configuration.GetConnectionString("PostgreSQLConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new VolunteerServiceException("Connection string 'PostgreSQLConnection' is not found.");
            }

            // Configure options
            var optionsBuilder = new DbContextOptionsBuilder<VolunteersDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new VolunteersDbContext(optionsBuilder.Options);
        }

    }
}
