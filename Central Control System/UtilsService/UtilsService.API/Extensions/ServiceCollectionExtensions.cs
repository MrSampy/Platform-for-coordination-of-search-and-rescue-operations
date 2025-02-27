using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UtilsService.Application.Interfaces;
using UtilsService.Application.Mapper;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;
using UtilsService.Persistence.DbContexts;
using UtilsService.Persistence.Repositories;

namespace UtilsService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            if (useInMemory)
            {
                services.AddDbContext<UtilsDbContext>(o => o.EnableSensitiveDataLogging()
                    .UseInMemoryDatabase("Test_Database"));
            }
            else
            {
                string connection = configuration.GetConnectionString("PostgreSQLConnection")!;
                services.AddDbContext<UtilsDbContext>(o => o.UseNpgsql(connection, sqlServerOptions =>
                    sqlServerOptions.EnableRetryOnFailure()));
            }

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("UtilsService.Application")));
            return services;
        }

        public static IServiceCollection AddMapperProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            return services;
        }

        public static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ICacheService<District>), typeof(CacheService<District>));

            services.AddSingleton(typeof(ICacheService<MeasurementUnit>), typeof(CacheService<MeasurementUnit>));

            services.AddSingleton(typeof(ICacheService<Resource>), typeof(CacheService<Resource>));

            services.AddSingleton(typeof(ICacheService<ResourceMeasurementUnit>), typeof(CacheService<ResourceMeasurementUnit>));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IDistrictRepository, DistrictRepository>();
            services.AddScoped<IMeasurementUnitRepository, MeasurementUnitRepository>();
            services.AddScoped<IResourceMeasurementUnitRepository, ResourceMeasurementUnitRepository>();
            services.AddScoped<IResourceRepository, ResourceRepository>();

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
