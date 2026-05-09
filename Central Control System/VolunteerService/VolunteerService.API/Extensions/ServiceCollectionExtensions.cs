using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using VolunteerService.API.Config;
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
        public static IServiceCollection AddAuthorizationJWT(this IServiceCollection services, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");
            if (!useInMemory)
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
                    }
                );

                services.AddCors();
            }

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

            var operationsServiceLink = useInMemory ? configuration.GetValue<string>(Constants.OperatrionsServiceInMemory) : configuration.GetValue<string>(Constants.OperatrionsService);

            var utilsServiceLink = useInMemory ? configuration.GetValue<string>(Constants.UtilsServiceInMemory) : configuration.GetValue<string>(Constants.UtilsService);

            if (!string.IsNullOrEmpty(authServiceLink))
            {
                services.AddHttpClient(Constants.AuthService, client =>
                {
                    client.BaseAddress = new Uri(authServiceLink);
                });
            }

            if (!string.IsNullOrEmpty(operationsServiceLink))
            {
                services.AddHttpClient(Constants.OperatrionsService, client =>
                {
                    client.BaseAddress = new Uri(operationsServiceLink);
                });
            }

            if (!string.IsNullOrEmpty(utilsServiceLink))
            {
                services.AddHttpClient(Constants.UtilsService, client =>
                {
                    client.BaseAddress = new Uri(utilsServiceLink);
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

            services.AddSingleton(typeof(ICacheService<VolunteersEvents>), typeof(CacheService<VolunteersEvents>));

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<Volunteer>), typeof(VolunteerRepository));

            services.AddScoped(typeof(IRepository<VolunteersDistricts>), typeof(VolunteersDistrictsRepository));

            services.AddScoped(typeof(IRepository<VolunteersGroups>), typeof(VolunteersGroupsRepository));

            services.AddScoped(typeof(IRepository<VolunteersEvents>), typeof(VolunteersEventsRepository));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
