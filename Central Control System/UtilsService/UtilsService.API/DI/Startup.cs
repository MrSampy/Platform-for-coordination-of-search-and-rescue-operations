using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using UtilsService.API.Extensions;
using UtilsService.API.Middleware;
using UtilsService.Persistence.DbContexts;

namespace UtilsService.API.DI
{
    internal sealed class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        private string[] controllers = new string[] { "District", "MeasurementUnit", "Resource", "ResourceMeasurementUnit" };

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRepositoryDbContext(Configuration);

            services.AddRepositories();

            services.AddMapperProfile();

            services.AddMediator();

            services.AddCacheServices();

            services.AddControllers();

            services.AddDistributedMemoryCache();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(swagger =>
            {
                foreach (var controllerName in controllers)
                {
                    swagger.SwaggerDoc(controllerName, new OpenApiInfo
                    {
                        Version = "v1",
                        Title = "API",
                        Description = "API",
                        Contact = new OpenApiContact
                        {
                            Name = "itsfinniii"
                        }
                    });
                }
            });

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            var useInMemory = configuration.GetValue<bool>("UseInMemoryDatabase");

            if (!useInMemory)
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    var context = services.GetRequiredService<UtilsDbContext>();
                    if (context.Database.GetPendingMigrations().Any()) context.Database.Migrate();
                }
            }

            app.UseRouting();

            app.UseMiddleware<ExceptionHandlingMiddleware>();
            app.UseMiddleware<RateLimitMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
