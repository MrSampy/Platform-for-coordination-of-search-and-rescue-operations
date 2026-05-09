using UtilsService.Domain.Entities;
using UtilsService.Persistence.DbContexts;

namespace UtilsService.API.Config
{
    public class DbSeeder
    {
        public static void Seed(UtilsDbContext context)
        {
            if (!context.Districts.Any())
            {
                context.AddRange(Constants.Districts);
            }

            if (!context.Resources.Any())
            {
                context.AddRange(Constants.Resources);
            }

            if (!context.MeasurementUnits.Any())
            {
                context.AddRange(Constants.MeasurementUnits);
            }

            if (!context.ResourceMeasurementUnits.Any())
            {
                context.AddRange(Constants.ResourceMeasurementUnits);
            }

            context.SaveChanges();
        }
    }
}
