using Microsoft.EntityFrameworkCore;
using System.Reflection;
using VolunteerService.Application.Mapper;
using VolunteerService.Application.Services;
using VolunteerService.Domain.Entities;
using VolunteerService.Domain.Interfaces;
using VolunteerService.Persistence.DbContexts;
using VolunteerService.Persistence.Repositories;

namespace VolunteerService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositoryDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            if (useInMemory)
            {
                services.AddDbContext<VolunteersDbContext>(o => o.EnableSensitiveDataLogging()
                    .UseInMemoryDatabase("Test_Database"));
            }
            else
            {
                string connection = configuration.GetConnectionString("PostgreSQLConnection")!;
                services.AddDbContext<VolunteersDbContext>(o => o.UseNpgsql(connection, sqlServerOptions =>
                    sqlServerOptions.EnableRetryOnFailure()));
            }

            return services;
        }

        public static IServiceCollection AddMediator(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.Load("VolunteerService.Application")));
            return services;
        }

        public static IServiceCollection AddMapperProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            return services;
        }

        public static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ICacheService<Volunteer>), typeof(CacheService<Volunteer>));

            services.AddSingleton(typeof(ICacheService<VolunteersDistricts>), typeof(CacheService<VolunteersDistricts>));

            services.AddSingleton(typeof(ICacheService<VolunteersGroups>), typeof(CacheService<VolunteersGroups>));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Volunteer>), typeof(VolunteerRepository));

            services.AddScoped(typeof(IRepository<VolunteersDistricts>), typeof(VolunteersDistrictsRepository));

            services.AddScoped(typeof(IRepository<VolunteersGroups>), typeof(VolunteersGroupsRepository));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
