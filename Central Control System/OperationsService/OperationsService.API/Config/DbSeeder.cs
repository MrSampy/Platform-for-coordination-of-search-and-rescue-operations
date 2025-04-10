using OperationsService.Domain.Entities;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.API.Config
{
    public class DbSeeder
    {
        public static void Seed(OperationsDbContext context)
        {
            if (!context.EventStatuses.Any())
            {
                var eventStatuses = Constants.EventStatuses;

                foreach (var eventStatus in eventStatuses)
                {
                    eventStatus.CreatedAt = eventStatus.UpdatedAt = DateTime.UtcNow;
                    context.EventStatuses.Add(eventStatus);
                }

            }
            if (!context.EventTypes.Any())
            {
                var eventTypes = Constants.EventTypes;

                foreach (var eventType in eventTypes)
                {
                    eventType.CreatedAt = eventType.UpdatedAt = DateTime.UtcNow;
                    context.EventTypes.Add(eventType);
                }
            }

            if (!context.OperationTaskStatuses.Any())
            {
                var taskStatuses = Constants.OperationTaskStatuses;

                foreach (var taskStatus in taskStatuses)
                {
                    taskStatus.CreatedAt = taskStatus.UpdatedAt = DateTime.UtcNow;
                    context.OperationTaskStatuses.Add(taskStatus);
                }

            }

            context.SaveChanges();
        }

    }
}
