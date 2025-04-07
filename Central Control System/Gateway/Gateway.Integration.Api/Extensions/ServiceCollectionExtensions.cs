using FluentValidation;
using Gateway.Application.Builders;
using Gateway.Application.Validators;
using Gateway.Domain.Services.Interfaces;
using Gateway.DTO.Constants;
using Gateway.Infrastructure.Services.Gateways;
using Gateway.Integration.Api.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text;

namespace Gateway.Integration.Api.Extensions
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

            var operationsServiceLink = useInMemory ? configuration.GetValue<string>(SharedConstants.OperatrionsServiceInMemory) : configuration.GetValue<string>(SharedConstants.OperatrionsService);

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
                services.AddHttpClient(SharedConstants.OperatrionsService, client =>
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
        public static IServiceCollection AddGateways(this IServiceCollection services)
        {
            services.AddSingleton<IAuthGateway, AuthGateway>();

            services.AddSingleton<IUtilsGateway, UtilsGateway>();

            return services;
        }
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<LoginModelValidator>();

            return services;
        }
    }
}
