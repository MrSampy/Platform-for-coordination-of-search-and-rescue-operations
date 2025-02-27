using UtilsService.API.Extensions;
using UtilsService.API.Middleware;

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
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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
