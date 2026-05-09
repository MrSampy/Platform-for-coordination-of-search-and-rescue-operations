using Gateway.Integration.Api.Middleware;

namespace Gateway.Integration.Api.Extensions
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
