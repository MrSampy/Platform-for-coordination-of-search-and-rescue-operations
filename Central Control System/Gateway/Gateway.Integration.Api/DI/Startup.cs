using Gateway.Integration.Api.Extensions;

namespace Gateway.Integration.Api.DI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        private string[] controllers = new string[] { "Authenticate", "Role", "User", "District", "MeasurementUnit",
                                                        "Resource", "ResourceMeasurementUnit",
                                                        "Volunteer", "VolunteersDistricts", "VolunteersGroups",
                                                        "Event", "EventStatus", "EventType", "Group",
                                                        "OperationTask", "OperationTaskStatus", "OperationWorker", "ResourcesEvent"};

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDistributedMemoryCache();

            services.AddEndpointsApiExplorer();

            services.AddHttpClients(Configuration);

            services.AddGateways();

            services.AddValidators();

            services.AddServices();

            services.AddAuthorizationJWT(Configuration);

            services.AddSwaggerDocWithAuth(controllers, Configuration);

            var result = services.SeedServices().Result;

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCustomCors(Configuration)
               .UseSwaggerWithEndpoints(env, controllers)
               .UseCustomMiddlewares()
               .ConfigureAuthAndRouting();
        }
    }
}
