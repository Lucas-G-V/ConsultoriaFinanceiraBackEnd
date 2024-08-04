using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XpInc.Logs
{
    public static class LogConfig
    {
        public static IServiceCollection AddLogHealthCheckConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationInsightsTelemetry(configuration?["ApplicationInsights:InstrumentationKey"]);

            services.AddHealthChecks()
                    .AddSqlServer(configuration.GetConnectionString("DefaultConnection"), name: "SQL Server")
                    .AddRedis(configuration.GetConnectionString("Redis"), name: "Redis");

            services.AddHealthChecksUI(setup =>
            {
                setup.SetEvaluationTimeInSeconds(60);
                setup.MaximumHistoryEntriesPerEndpoint(50);
                setup.SetApiMaxActiveRequests(1);
            }).AddInMemoryStorage();


            return services;
        }

        public static void UseHealthChecksConfiguration(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/health", new HealthCheckOptions
                {
                    Predicate = _ => true,
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.UIPath = "/health-ui";
                    setup.ApiPath = "/health-ui-api";
                });

                endpoints.MapControllers();
            });
        }
    }
}
