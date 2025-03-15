using UtilsService.API.Extensions;

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

            services.AddHttpClients(Configuration);

            services.AddAuthorizationJWT(Configuration);

            services.AddSwaggerDocWithAuth(controllers, Configuration);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomCors(Configuration)
               .UseSwaggerWithEndpoints(env, controllers)
               .ApplyDatabaseMigrations(Configuration)
               .UseCustomMiddlewares()
               .ConfigureAuthAndRouting();
        }
    }
}
