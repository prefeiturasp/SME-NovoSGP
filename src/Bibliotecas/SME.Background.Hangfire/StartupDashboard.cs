using Hangfire;
using Hangfire.Dashboard;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace SME.Background.Hangfire
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseHangfireDashboard("/worker", new DashboardOptions()
            {
                IsReadOnlyFunc = (DashboardContext context) => true
            },
            new PostgreSqlStorage(Configuration.GetConnectionString("SGP-Postgres"), new PostgreSqlStorageOptions()
            {
                SchemaName = "hangfire"
            }));

        }

    }
}
