using Microsoft.EntityFrameworkCore;
using OperationsService.Application.Mapper;
using OperationsService.Application.Services;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;
using OperationsService.Persistence.Repositories;
using System.Reflection;

namespace OperationsService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            if (useInMemory)
            {
                services.AddDbContext<OperationsDbContext>(o => o.EnableSensitiveDataLogging()
                    .UseInMemoryDatabase("Test_Database"));
            }
            else
            {
                string connection = configuration.GetConnectionString("PostgreSQLConnection")!;
                services.AddDbContext<OperationsDbContext>(o => o.UseNpgsql(connection, sqlServerOptions =>
                    sqlServerOptions.EnableRetryOnFailure()));
            }

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("OperationsService.Application")));
            return services;
        }

        public static IServiceCollection AddMapperProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            return services;
        }

        public static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ICacheService<Event>), typeof(CacheService<Event>));

            services.AddSingleton(typeof(ICacheService<EventStatus>), typeof(CacheService<EventStatus>));

            services.AddSingleton(typeof(ICacheService<EventType>), typeof(CacheService<EventType>));

            services.AddSingleton(typeof(ICacheService<Group>), typeof(CacheService<Group>));

            services.AddSingleton(typeof(ICacheService<OperationTask>), typeof(CacheService<OperationTask>));

            services.AddSingleton(typeof(ICacheService<OperationTaskStatus>), typeof(CacheService<OperationTaskStatus>));

            services.AddSingleton(typeof(ICacheService<OperationWorker>), typeof(CacheService<OperationWorker>));

            services.AddSingleton(typeof(ICacheService<ResourcesEvent>), typeof(CacheService<ResourcesEvent>));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Event>), typeof(EventRepository));

            services.AddScoped(typeof(IRepository<EventStatus>), typeof(EventStatusRepository));

            services.AddScoped(typeof(IRepository<EventType>), typeof(EventTypeRepository));

            services.AddScoped(typeof(IRepository<Group>), typeof(GroupRepository));

            services.AddScoped(typeof(IRepository<OperationTask>), typeof(OperationTaskRepository));

            services.AddScoped(typeof(IRepository<OperationTaskStatus>), typeof(OperationTaskStatusRepository));

            services.AddScoped(typeof(IRepository<OperationWorker>), typeof(OperationWorkerRepository));

            services.AddScoped(typeof(IRepository<ResourcesEvent>), typeof(ResourcesEventRepository));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
