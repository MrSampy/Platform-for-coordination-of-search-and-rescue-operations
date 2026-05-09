using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using UtilsService.API.Config;
using UtilsService.Application.Interfaces;
using UtilsService.Application.Mapper;
using UtilsService.Application.Services;
using UtilsService.Domain.Entities;
using UtilsService.Domain.Interfaces;
using UtilsService.Persistence.DbContexts;
using UtilsService.Persistence.Repositories;

namespace UtilsService.API.Extensions
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
            var authServiceLink = configuration.GetValue<string>(Constants.AuthService);

            if (!string.IsNullOrEmpty(authServiceLink))
            {
                services.AddHttpClient(Constants.AuthService, client =>
                {
                    client.BaseAddress = new Uri(authServiceLink);
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
