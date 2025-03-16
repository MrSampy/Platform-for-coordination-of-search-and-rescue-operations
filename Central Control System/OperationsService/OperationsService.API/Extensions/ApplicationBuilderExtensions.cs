using Microsoft.EntityFrameworkCore;
using OperationsService.API.Middleware;
using OperationsService.Persistence.DbContexts;

namespace OperationsService.API.Extensions
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseCors(
                b => b
                    .WithOrigins((configuration["OriginUrls"] ?? string.Empty).Split(','))
                    .SetIsOriginAllowedToAllowWildcardSubdomains()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
            );
            return app;
        }

        public static IApplicationBuilder UseSwaggerWithEndpoints(this IApplicationBuilder app, IWebHostEnvironment env, string[] controllers)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    foreach (var controllerName in controllers)
                    {
                        c.SwaggerEndpoint($"/swagger/{controllerName}/swagger.json", controllerName);
                    }
                });
            }
            return app;
        }

        public static IApplicationBuilder ApplyDatabaseMigrations(this IApplicationBuilder app, IConfiguration configuration)
        {
            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            if (!useInMemory)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    var context = services.GetRequiredService<OperationsDbContext>();

                    if (context.Database.GetPendingMigrations().Any())
                    {
                        context.Database.Migrate();
                    }
                }
            }
            return app;
        }

        public static IApplicationBuilder UseCustomMiddlewares(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RateLimitMiddleware>();
            return app;
        }

        public static IApplicationBuilder ConfigureAuthAndRouting(this IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            return app;
        }
    }
}
