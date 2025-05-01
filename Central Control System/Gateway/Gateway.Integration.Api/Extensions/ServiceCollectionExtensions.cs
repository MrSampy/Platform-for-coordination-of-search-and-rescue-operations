using FluentValidation;
using Gateway.Application.Builders;
using Gateway.Application.Consumers;
using Gateway.Application.Filters;
using Gateway.Application.Mapper;
using Gateway.Application.Validators;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.DTO.DTOs.Operations;
using Gateway.DTO.DTOs.Utils;
using Gateway.DTO.DTOs.Volunteers;
using Gateway.Infrastructure.Services.Gateways;
using Gateway.Infrastructure.Services.Gateways.Gateway.Infrastructure.Services.Gateways;
using Gateway.Infrastructure.Services.Publishers;
using Gateway.Infrastructure.Services.Services;
using Gateway.Integration.Api.Config;
using Gateway.Integration.Api.Config.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using Swashbuckle.AspNetCore.Filters;
using System.Text;

namespace Gateway.Integration.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static async Task<IServiceCollection> SeedServices(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var authGateway = serviceProvider.GetRequiredService<IAuthGateway>();
            var volunteersGateway = serviceProvider.GetRequiredService<IVolunteersGateway>();
            var operationsGateway = serviceProvider.GetRequiredService<IOperationsGateway>();
            var seeder = new DbSeeder(authGateway, operationsGateway, volunteersGateway);

            await seeder.SeedAsync();

            return services;
        }
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
            services.AddSwaggerExamplesFromAssemblyOf<LoginModelExample>();

            services.AddSwaggerGen(swagger =>
            {
                swagger.ExampleFilters();
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

                swagger.SwaggerDoc("Gateway", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Gateway",
                    Description = "Gateway.API",
                    Contact = new OpenApiContact { Name = "itsfinniii" }
                });


                swagger.DocInclusionPredicate((docName, apiDesc) =>
                {
                    if (docName == "Authentication")
                    {
                        return apiDesc.ActionDescriptor.RouteValues["controller"] == "Token";
                    }

                    if (apiDesc.ActionDescriptor.RouteValues["controller"] == "Token")
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

            var authServiceLink = useInMemory ? configuration.GetValue<string>(SharedConstants.AuthServiceInMemory) : configuration.GetValue<string>(SharedConstants.AuthService);

            var utilsServiceLink = useInMemory ? configuration.GetValue<string>(SharedConstants.UtilsServiceInMemory) : configuration.GetValue<string>(SharedConstants.UtilsService);

            var volunteerServiceLink = useInMemory ? configuration.GetValue<string>(SharedConstants.VolunteerServiceInMemory) : configuration.GetValue<string>(SharedConstants.VolunteerService);

            var operationsServiceLink = useInMemory ? configuration.GetValue<string>(SharedConstants.OperationsServiceInMemory) : configuration.GetValue<string>(SharedConstants.OperationsService);

            if (!string.IsNullOrEmpty(authServiceLink))
            {
                services.AddHttpClient(SharedConstants.AuthService, client =>
                {
                    client.BaseAddress = new Uri(authServiceLink);
                });
            }

            if (!string.IsNullOrEmpty(utilsServiceLink))
            {
                services.AddHttpClient(SharedConstants.UtilsService, client =>
                {
                    client.BaseAddress = new Uri(utilsServiceLink);
                });
            }

            if (!string.IsNullOrEmpty(operationsServiceLink))
            {
                services.AddHttpClient(SharedConstants.OperationsService, client =>
                {
                    client.BaseAddress = new Uri(operationsServiceLink);
                });
            }

            if (!string.IsNullOrEmpty(volunteerServiceLink))
            {
                services.AddHttpClient(SharedConstants.VolunteerService, client =>
                {
                    client.BaseAddress = new Uri(volunteerServiceLink);
                });
            }

            services.AddTransient<IApiBuilder, ApiBuilder>();

            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IOperationsService, OperationsService>();

            services.AddTransient<IVolunteersService, VolunteersService>();

            services.AddTransient<IQRCodeService, QRCodeService>();

            services.AddSingleton<IConnectionFactory>(sp =>
            {
                var config = sp.GetRequiredService<IConfiguration>().GetSection("RabbitMQ");
                return new ConnectionFactory
                {
                    HostName = config["Host"],
                    UserName = config["Username"],
                    Password = config["Password"],
                    DispatchConsumersAsync = true
                };
            });

            services.AddSingleton<IConnection>(sp =>
            {
                var factory = sp.GetRequiredService<IConnectionFactory>();
                return factory.CreateConnection();
            });

            services.AddSingleton<IModel>(sp =>
            {
                var connection = sp.GetRequiredService<IConnection>();
                return connection.CreateModel();
            });

            services.AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();

            services.AddHostedService<SendMessageConsumer>();

            return services;
        }
        public static IServiceCollection AddCache(this IServiceCollection services)
        {
            services.AddSingleton<ICacheService<DistrictDTO>, CacheService<DistrictDTO>>();
            services.AddSingleton<ICacheService<ResourceDTO>, CacheService<ResourceDTO>>();
            services.AddSingleton<ICacheService<MeasurementUnitDTO>, CacheService<MeasurementUnitDTO>>();
            services.AddSingleton<ICacheService<VolunteerDTO>, CacheService<VolunteerDTO>>();
            services.AddSingleton<ICacheService<VolunteersDistrictsDTO>, CacheService<VolunteersDistrictsDTO>>();
            services.AddSingleton<ICacheService<VolunteersGroupsDTO>, CacheService<VolunteersGroupsDTO>>();
            services.AddSingleton<ICacheService<VolunteersEventsDTO>, CacheService<VolunteersEventsDTO>>();

            services.AddSingleton<ICacheService<MessageDTO>, CacheService<MessageDTO>>();
            services.AddSingleton<ICacheService<EventDTO>, CacheService<EventDTO>>();
            services.AddSingleton<ICacheService<EventStatusDTO>, CacheService<EventStatusDTO>>();
            services.AddSingleton<ICacheService<EventTypeDTO>, CacheService<EventTypeDTO>>();
            services.AddSingleton<ICacheService<GroupDTO>, CacheService<GroupDTO>>();
            services.AddSingleton<ICacheService<OperationTaskDTO>, CacheService<OperationTaskDTO>>();
            services.AddSingleton<ICacheService<OperationTaskStatusDTO>, CacheService<OperationTaskStatusDTO>>();
            services.AddSingleton<ICacheService<OperationWorkerDTO>, CacheService<OperationWorkerDTO>>();
            services.AddSingleton<ICacheService<ResourcesEventDTO>, CacheService<ResourcesEventDTO>>();

            return services;
        }
        public static IServiceCollection AddGateways(this IServiceCollection services)
        {
            services.AddTransient<IAuthGateway, AuthGateway>();

            services.AddTransient<IUtilsGateway, UtilsGateway>();

            services.AddTransient<IVolunteersGateway, VolunteersGateway>();

            services.AddTransient<IOperationsGateway, OperationsGateway>();

            return services;
        }
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<LoginModelValidator>();

            return services;
        }
        public static IServiceCollection AddMapperProfile(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);
            return services;
        }
    }
}
