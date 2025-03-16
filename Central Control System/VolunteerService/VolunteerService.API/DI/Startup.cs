using VolunteerService.API.Extensions;

namespace VolunteerService.API.DI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        private string[] _controllers = ["Volunteer", "VolunteersDistricts", "VolunteersGroups"];

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

            services.AddSwaggerDocWithAuth(_controllers, Configuration);

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomCors(Configuration)
               .UseSwaggerWithEndpoints(env, _controllers)
               .ApplyDatabaseMigrations(Configuration)
               .UseCustomMiddlewares()
               .ConfigureAuthAndRouting();
        }
    }
}
