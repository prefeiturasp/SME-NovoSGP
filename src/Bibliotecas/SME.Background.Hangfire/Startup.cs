using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SME.Background.Hangfire
{
    public class Startup
    {
        private readonly IConfiguration configuration;
        private readonly string connectionString;

        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
            var paramConnectionString = this.configuration.GetConnectionString("SGP-Postgres");
            this.connectionString = (!paramConnectionString.EndsWith(';') ? paramConnectionString + ";" : paramConnectionString) + "Application Name=SGP Worker Service Dashboard";
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHangfireDashboard("/worker", new DashboardOptions()
            {
                IsReadOnlyFunc = (DashboardContext context) => !env.IsDevelopment(),
                Authorization = new[] { new DashboardAuthorizationFilter() },
                StatsPollingInterval = 10000 // atualiza a cada 10s
            });
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHangfire(configuration => configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseFilter<AutomaticRetryAttribute>(new AutomaticRetryAttribute() { Attempts = 0 })
                .UsePostgreSqlStorage(connectionString, new PostgreSqlStorageOptions()
                {
                    SchemaName = "hangfire"
                }));
        }
    }
}