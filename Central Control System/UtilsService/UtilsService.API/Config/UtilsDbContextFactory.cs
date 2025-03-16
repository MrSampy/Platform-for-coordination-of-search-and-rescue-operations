using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using UtilsService.Persistence.DbContexts;

namespace UtilsService.API.Config
{
    public class UtilsDbContextFactory : IDesignTimeDbContextFactory<UtilsDbContext>
    {
        public UtilsDbContext CreateDbContext(string[] args)
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
                throw new InvalidOperationException("Connection string 'PostgreSQLConnection' is not found.");
            }

            // Configure options
            var optionsBuilder = new DbContextOptionsBuilder<UtilsDbContext>();
            optionsBuilder.UseNpgsql(connectionString);

            return new UtilsDbContext(optionsBuilder.Options);
        }
    }

}
