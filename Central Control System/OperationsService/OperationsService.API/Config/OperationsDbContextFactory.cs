using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using OperationsService.Domain.Exceptions;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.API.Config
{
    public class OperationsDbContextFactory : IDesignTimeDbContextFactory<OperationsDbContext>
    {
        public OperationsDbContext CreateDbContext(string[] args)
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
                throw new OperationsServiceException("Connection string 'PostgreSQLConnection' is not found.");
            }

            // Configure options
            var optionsBuilder = new DbContextOptionsBuilder<OperationsDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new OperationsDbContext(optionsBuilder.Options);
        }
    }
}
