using Gateway.Integration.Api.Extensions;

namespace Gateway.Integration.Api.DI
{
    public class Startup(IConfiguration configuration)
    {
        public IConfiguration Configuration { get; } = configuration;

        private string[] controllers = new string[] { "Authenticate", "Role", "User", "District", "MeasurementUnit",
                                                        "Resource", "ResourceMeasurementUnit",
                                                        "Volunteer", "VolunteersDistricts", "VolunteersGroups", "VolunteersEvents",
                                                        "Event", "EventStatus", "EventType", "Group",
                                                        "OperationTask", "OperationTaskStatus", "OperationWorker", "ResourcesEvent", "Message"};

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddDistributedMemoryCache();

            services.AddEndpointsApiExplorer();

            services.AddHttpClients(Configuration);

            services.AddCache();

            services.AddGateways();

            services.AddValidators();

            services.AddMapperProfile();

            services.AddServices();

            services.AddAuthorizationJWT(Configuration);

            services.AddSwaggerDocWithAuth(controllers, Configuration);

            try
            {
                var result = services.SeedServices().Result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
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
