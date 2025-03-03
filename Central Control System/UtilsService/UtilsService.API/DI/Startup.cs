using Microsoft.EntityFrameworkCore;
using UtilsService.API.Extensions;
using UtilsService.API.Middleware;
using UtilsService.Persistence.DbContexts;

namespace UtilsService.API.DI
{
    internal sealed class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;
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

            services.AddSwaggerGen();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
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
