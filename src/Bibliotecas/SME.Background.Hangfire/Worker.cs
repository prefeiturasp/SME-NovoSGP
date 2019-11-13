using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using SME.Background.Core.Interfaces;
using System;
using System.IO;

namespace SME.Background.Hangfire
{
    public class Worker : IWorker
    {
        readonly IConfiguration configuration;
        BackgroundJobServer hangFireServer;
        IWebHost host;

        public Worker(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Dispose()
        {
            hangFireServer?.Dispose();
            host?.Dispose();
        }

        public void Registrar()
        {
            host = new WebHostBuilder()
               .UseKestrel()
               .UseContentRoot(Directory.GetCurrentDirectory())
               .ConfigureAppConfiguration((hostContext, config) =>
               {
                   config.SetBasePath(Directory.GetCurrentDirectory());
                   config.AddEnvironmentVariables();
               })
               .ConfigureServices((hostContext, services) =>
               {
                   services.AddHangfire(configuration => configuration
                       .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                       .UseSimpleAssemblyNameTypeSerializer()
                       .UseRecommendedSerializerSettings()
                       .UseFilter<AutomaticRetryAttribute>(new AutomaticRetryAttribute() { Attempts = 0 })
                       .UsePostgreSqlStorage(hostContext.Configuration.GetConnectionString("SGP-Postgres"), new PostgreSqlStorageOptions()
                       {
                           QueuePollInterval = TimeSpan.FromSeconds(1),
                           InvisibilityTimeout = TimeSpan.FromMinutes(1),
                           SchemaName = "hangfire"
                       }));
                   services.AddHangfireServer();
               })
               .UseStartup<Startup>()
               .Build();

            host.RunAsync();
        }
    }
}
//