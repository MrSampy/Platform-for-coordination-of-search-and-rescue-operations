using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OperationsService.API.Config;
using OperationsService.Application.Mapper;
using OperationsService.Application.Services;
using OperationsService.Domain.Entities;
using OperationsService.Domain.Interfaces;
using OperationsService.Persistence.DbContexts;
using OperationsService.Persistence.Repositories;
using System.Reflection;
using System.Text;

namespace OperationsService.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAuthorizationJWT(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = configuration["ValidAudience"],
                        ValidIssuer = configuration["ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Secret"] ?? string.Empty))
                    };
                });

            services.AddCors();

            return services;
        }

        public static IServiceCollection AddSwaggerDocWithAuth(this IServiceCollection services, string[] controllers, IConfiguration configuration)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.OperationFilter<AuthenticationHeadersFilter>();

                var securityScheme = new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter 'Bearer {token}'",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = JwtBearerDefaults.AuthenticationScheme
                    }
                };

                swagger.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, securityScheme);

                swagger.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        securityScheme,
                        new List<string>()
                    }
                });

                foreach (var controllerName in controllers)
                {
                    swagger.SwaggerDoc(controllerName, new OpenApiInfo
                    {
                        Version = "v1",
                        Title = $"API - {controllerName}",
                        Description = "API",
                        Contact = new OpenApiContact
                        {
                            Name = "itsfinniii"
                        }
                    });
                }

                swagger.SwaggerDoc("Authentication", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Authentication",
                    Description = "Token Generation API",
                    Contact = new OpenApiContact { Name = "itsfinniii" }
                });


                swagger.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (docName == "Authentication")
                    {
                        return apiDesc.ActionDescriptor.RouteValues["controller"] == "Authenticate";
                    }

                    if (apiDesc.ActionDescriptor.RouteValues["controller"] == "Authenticate")
                    {
                        return true;
                    }

                    return controllers.Contains(docName) && apiDesc.GroupName == docName;
                });
            });

            return services;
        }

        public static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            var authServiceLink = useInMemory ? configuration.GetValue<string>(Constants.AuthServiceInMemory) : configuration.GetValue<string>(Constants.AuthService);

            var utilsServiceLink = useInMemory ? configuration.GetValue<string>(Constants.UtilsServiceInMemory) : configuration.GetValue<string>(Constants.UtilsService);

            var volunteerServiceLink = useInMemory ? configuration.GetValue<string>(Constants.VolunteerServiceInMemory) : configuration.GetValue<string>(Constants.VolunteerService);

            if (!string.IsNullOrEmpty(authServiceLink))
            {
                services.AddHttpClient(Constants.AuthService, client =>
                {
                    client.BaseAddress = new Uri(authServiceLink);
                });
            }

            if (!string.IsNullOrEmpty(utilsServiceLink))
            {
                services.AddHttpClient(Constants.UtilsService, client =>
                {
                    client.BaseAddress = new Uri(utilsServiceLink);
                });
            }

            if (!string.IsNullOrEmpty(volunteerServiceLink))
            {
                services.AddHttpClient(Constants.VolunteerService, client =>
                {
                    client.BaseAddress = new Uri(volunteerServiceLink);
                });
            }

            services.AddTransient<IApiBuilder, ApiBuilder>();

            return services;
        }

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
