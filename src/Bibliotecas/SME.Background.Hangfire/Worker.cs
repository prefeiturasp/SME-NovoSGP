using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SME.Background.Core.Interfaces;
using System;
using System.IO;
using System.Linq;

namespace SME.Background.Hangfire
{
    public class Worker : IWorker
    {
        readonly IConfiguration configuration;
        IWebHost host;
        readonly Action<IServiceCollection> registryDI;

        public Worker(IConfiguration configuration, Action<IServiceCollection> registryDI)
        {
            this.configuration = configuration;
            this.registryDI = registryDI;
        }

        public void Dispose()
        {
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
               .ConfigureServices(x=> registryDI(x))
               .UseStartup<Startup>()
               .Build();

            host.RunAsync();
        }
    }
}
//