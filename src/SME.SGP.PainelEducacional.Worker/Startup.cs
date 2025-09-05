using Elastic.Apm.AspNetCore;
using Elastic.Apm.DiagnosticSource;
using Elastic.Apm.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SME.SGP.Infra;
using SME.SGP.IoC;
using SME.SGP.IoC.Extensions;

namespace SME.SGP.PainelEducacional.Worker
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var registrarDependencias = new RegistrarDependencias();
            registrarDependencias.RegistrarParaWorkers(services, Configuration);
            registrarDependencias.RegistrarCasoDeUsoPainelEducacionalRabbitSgp(services);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseElasticApm(Configuration,
                new SqlClientDiagnosticSubscriber(),
                new HttpDiagnosticsSubscriber());

            app.UseHealthChecksSgp();
            app.UseHealthCheckPrometheusSgp();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            RegistrarConfigsThreads.Registrar(Configuration);

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("WorkerRabbitPainelEducacional!");
            });
        }
    }
}
