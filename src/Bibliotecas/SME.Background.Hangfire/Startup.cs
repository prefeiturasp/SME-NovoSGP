using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace SME.Background.Hangfire
{
    public class Startup
    {
        IConfiguration configuration;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            GlobalConfiguration.Configuration.UseActivator<HangfireActivator>(new HangfireActivator(app.ApplicationServices));

            app.UseHangfireDashboard("/worker", new DashboardOptions()
            {
                IsReadOnlyFunc = (DashboardContext context) => true
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseFilter<AutomaticRetryAttribute>(new AutomaticRetryAttribute() { Attempts = 0 })
                .UsePostgreSqlStorage(this.configuration.GetConnectionString("SGP-Postgres"), new PostgreSqlStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromSeconds(1),
                    InvisibilityTimeout = TimeSpan.FromMinutes(1),
                    SchemaName = "hangfire"
                }));
            services.AddHangfireServer();
        }

    }
}
