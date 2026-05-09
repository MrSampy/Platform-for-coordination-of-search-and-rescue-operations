using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace OperationsService.API.Config
{
    public class AuthenticationHeadersFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var hasAuthHeader = false;

            // Check if the method has RequiresAuthHeader attribute
            if (context.MethodInfo.GetCustomAttributes(typeof(RequiresAuthHeaderAttribute), false).Any())
            {
                hasAuthHeader = true;
            }

            // Check if the controller (declaring type) has RequiresAuthHeader attribute
            var controllerActionDescriptor = context.ApiDescription.ActionDescriptor as ControllerActionDescriptor;
            if (controllerActionDescriptor != null &&
                controllerActionDescriptor.ControllerTypeInfo.GetCustomAttributes(typeof(RequiresAuthHeaderAttribute), false).Any())
            {
                hasAuthHeader = true;
            }

            if (!hasAuthHeader)
            {
                return; // Skip adding the parameter if the attribute is not present
            }

            if (operation.Security == null)
                operation.Security = new List<OpenApiSecurityRequirement>();

            var jwtSecurityScheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = JwtBearerDefaults.AuthenticationScheme
                }
            };

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [jwtSecurityScheme] = new List<string>()
            });
        }
    }
}
