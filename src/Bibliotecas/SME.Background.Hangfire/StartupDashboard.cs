using System.Web.Http;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Hangfire.AspNetCore;
using Hangfire.Dashboard;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Globalization;
using Microsoft.AspNetCore.Hosting;
using Hangfire.PostgreSql;
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
